#FROM ollama/ollama:rocm
#WORKDIR /root/.ollama
#EXPOSE 11434
#CMD ["ollama", "run", "llama3.1:8b"]

# Dockerfile for embedding model
FROM ollama/ollama:rocm

# Set up the working directory
WORKDIR /root/.ollama

# Expose the port your application runs on
EXPOSE 11434

# Command to run the embedding model
CMD ["ollama", "run", "llama3.1:8b"]