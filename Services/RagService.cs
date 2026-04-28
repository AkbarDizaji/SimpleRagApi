using SimpleRagApi.Models;

namespace SimpleRagApi.Services;

public class RagService
{
    private readonly DocumentChunker _chunker;
    private readonly GeminiService _gemini;
    private readonly VectorStore _vectorStore;

    public RagService(DocumentChunker chunker, GeminiService gemini, VectorStore vectorStore)
    {
        _chunker = chunker;
        _gemini = gemini;
        _vectorStore = vectorStore;
    }

    public async Task<int> IndexDocumentAsync(string text)
    {
        var chunks = _chunker.Chunk(text);
        var records = new List<ChunkRecord>();

        foreach (var chunkText in chunks)
        {
            var embedding = await _gemini.GetEmbeddingAsync(chunkText);
            records.Add(new ChunkRecord
            {
                Id = Guid.NewGuid().ToString(),
                Text = chunkText,
                Embedding = embedding
            });
        }

        var existing = _vectorStore.GetAll();
        existing.AddRange(records);
        _vectorStore.SaveAll(existing);

        return records.Count;
    }

    public async Task<(string Answer, List<string> ContextChunks)> AskAsync(string question)
    {
        var queryEmbedding = await _gemini.GetEmbeddingAsync(question);
        var allChunks = _vectorStore.GetAll();

        var topChunks = allChunks
            .Select(c => new { Chunk = c, Similarity = _vectorStore.CosineSimilarity(queryEmbedding, c.Embedding) })
            .OrderByDescending(x => x.Similarity)
            .Take(3)
            .ToList();

        var context = string.Join("\n---\n", topChunks.Select(x => x.Chunk.Text));
        var answer = await _gemini.AskAsync(question, context);

        return (answer, topChunks.Select(x => x.Chunk.Text).ToList());
    }
}
