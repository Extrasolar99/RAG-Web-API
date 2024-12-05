# RAG Web API

This is a Dotnet Web API with RAG and a Postgresql vector database.


## How to set up

- The docker-compose file takes care of spinning up the API and vector database.
- Download ollama and pull the `mxbai-embed-large` model.
- Run ollama by executing `ollama serve`.

At the moment, the API container connects to the local machine to access ollama, in the future an ollama container should be used.

## Current vs. Planned functionality

### Current

- Allows connections between the api, database and an embedding model.
- Is capable of converting text to vector data.
- Implements a generative AI model to converse with the using using an endpoint
- Routes the prompt between the embedding and generative models to produce a response

### Planned

- Integration of a generative LLM -> DONE
- Endpoints for storing various types of data (for ex. pdf's, maybe images etc...)
- Move embedding (and generating) model(s) to container(s) as well -> PAIN IN THE ASS
- Probably a lot more :P