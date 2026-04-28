# Simple RAG API MVP (Gemini Version)

A simple, working MVP of a Retrieval-Augmented Generation (RAG) system built with .NET 8 and Google Gemini.

## Features
- **Document Indexing**: Splits text into chunks, generates embeddings via Google Gemini, and stores them locally in a JSON file.
- **Question Answering**: Generates an embedding for the user's question, retrieves the most relevant chunks using cosine similarity, and generates a grounded answer using Gemini Chat.
- **Local Storage**: Uses `Data/chunks.json` for persistent storage of embeddings (no database required).

## Prerequisites
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- Google Gemini API Key (Free tier available at [aistudio.google.com](https://aistudio.google.com/))

## Configuration
Update `appsettings.json` with your Gemini details:

```json
{
  "Gemini": {
    "ApiKey": "YOUR_GEMINI_API_KEY",
    "ChatModel": "gemini-2.5-flash",
    "EmbeddingModel": "gemini-embedding-001"
  }
}
```

## How to Run
1. Run the project from the root folder:
   ```bash
   dotnet run
   ```
2. The API will be available at `http://localhost:5000/swagger`.

## Example Usage

### 1. Index a Document
```bash
curl -X POST http://localhost:5000/rag/index \
-H "Content-Type: application/json" \
-d '{
  "text": "The company refund policy allows for full refunds within 30 days of purchase. To request a refund, customers must provide a valid receipt and the item must be in its original packaging. Refunds are processed within 5-7 business days."
}'
```

### 2. Ask a Question
```bash
curl -X POST http://localhost:5000/rag/ask \
-H "Content-Type: application/json" \
-d '{
  "question": "What is the refund policy?"
}'
```

## Project Structure
- `Controllers/RagController.cs`: API endpoints for indexing and asking.
- `Services/GeminiService.cs`: Wrapper for Google Gemini REST API calls.
- `Services/DocumentChunker.cs`: Logic for splitting text into overlapping chunks.
- `Services/VectorStore.cs`: Manages local JSON storage and cosine similarity.
- `Services/RagService.cs`: Orchestrates the RAG flow.
- `Data/chunks.json`: Local file where indexed data is stored.

## Note
This is an MVP designed for simplicity and readability. It uses local JSON file storage and basic cosine similarity, which is suitable for small datasets but not intended for production-scale vector search.
