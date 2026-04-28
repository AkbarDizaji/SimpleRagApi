using System.Text.Json;
using SimpleRagApi.Models;

namespace SimpleRagApi.Services;

public class VectorStore
{
    private readonly string _filePath;

    public VectorStore()
    {
        var dataDir = Path.Combine(Directory.GetCurrentDirectory(), "Data");
        if (!Directory.Exists(dataDir))
        {
            Directory.CreateDirectory(dataDir);
        }
        _filePath = Path.Combine(dataDir, "chunks.json");
    }

    public List<ChunkRecord> GetAll()
    {
        if (!File.Exists(_filePath)) return [];

        var json = File.ReadAllText(_filePath);
        return JsonSerializer.Deserialize<List<ChunkRecord>>(json) ?? [];
    }

    public void SaveAll(List<ChunkRecord> chunks)
    {
        var json = JsonSerializer.Serialize(chunks, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(_filePath, json);
    }

    public float CosineSimilarity(float[] a, float[] b)
    {
        if (a.Length != b.Length) return 0;

        float dotProduct = 0;
        float normA = 0;
        float normB = 0;

        for (int i = 0; i < a.Length; i++)
        {
            dotProduct += a[i] * b[i];
            normA += a[i] * a[i];
            normB += b[i] * b[i];
        }

        if (normA == 0 || normB == 0) return 0;

        return dotProduct / (MathF.Sqrt(normA) * MathF.Sqrt(normB));
    }
}
