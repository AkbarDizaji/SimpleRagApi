namespace SimpleRagApi.Models;

public class ChunkRecord
{
    public string Id { get; set; } = string.Empty;
    public string Text { get; set; } = string.Empty;
    public float[] Embedding { get; set; } = [];
}
