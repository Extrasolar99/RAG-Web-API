#!/bin/sh

# Pull the embedding model
ollama pull mxbai-embed-large

# Pull the generative model
ollama pull llama3.1:8b

# Check if the models are pulled successfully
while ! ollama list | grep -q "mxbai-embed-large"; do
  echo "Waiting for mxbai-embed-large model to be pulled..."
  sleep 5
done

while ! ollama list | grep -q "llama3.1:8b"; do
  echo "Waiting for llama3.1:8b model to be pulled..."
  sleep 5
done

# Start the service
exec "$@"