namespace SimpleRagApi.Services;

public class DocumentChunker
{
    public List<string> Chunk(string text, int chunkSize = 700, int overlap = 100)
    {
        if (string.IsNullOrWhiteSpace(text)) return [];

        var chunks = new List<string>();
        int start = 0;

        while (start < text.Length)
        {
            int length = Math.Min(chunkSize, text.Length - start);
            var chunk = text.Substring(start, length).Trim();
            
            if (!string.IsNullOrWhiteSpace(chunk))
            {
                chunks.Add(chunk);
            }

            if (start + length >= text.Length) break;
            
            start += (chunkSize - overlap);
        }

        return chunks;
    }
}
