# Developer Evaluation Project
 
`READ CAREFULLY`
 
## Instructions
**The test below will have up to 5 calendar days to be delivered from the date of receipt of this manual.**
 
- The code must be versioned in a public Github repository and a link must be sent for evaluation once completed
- Upload this template to your repository and start working from it
- Read the instructions carefully and make sure all requirements are being addressed
- Documentation and overall organization will also be taken into consideration
 
## Use Case
**You are a developer on the DeveloperStore team. Now we need to implement the API prototypes.**
 
As we work with `DDD`, to reference entities from other domains, we use the `External Identities` pattern with denormalization of entity descriptions.
 
Therefore, you will write an API (complete CRUD) that handles sales records. The API needs to be able to inform:
 
* Sale number
* Date when the sale was made
* Customer
* Total sale amount (with the taxes applied)
* Branch where the sale was made
* cancelled true/false
* Products
* Quantities
* Unit prices
* value of Tax applied
* Total amount for each item
 
 
If you write the code, **it's not required** to actually publish to any Message Broker. You can log a message in the application log or however you find most convenient.
 
### Business Rules
 
* Purchases above 4 identical items have a IVA TAX
* Purchases between 10 and 20 identical items have a SPECIAL IVA
* It's not possible to sell above 20 identical items
* Purchases below 4 items have a free TAX
 
These business rules define quantity-based taxing tiers and limitations:
 
1. tax Tiers:
   - IVA = 10% 
   - SPECIAL IVA = 20%
 
## Overview
This section provides a high-level overview of the project and the various skills and competencies it aims to assess for developer candidates.
 
See [Overview](/.doc/overview.md)
 
## Tech Stack
This section lists the key technologies used in the project, including the backend, testing, frontend, and database components.
 
See [Tech Stack](/.doc/tech-stack.md)
 
## Frameworks
This section outlines the frameworks and libraries that are leveraged in the project to enhance development productivity and maintainability.
 
See [Frameworks](/.doc/frameworks.md)
 
<!--
## API Structure
This section includes links to the detailed documentation for the different API resources:
- [API General](./docs/general-api.md)
-->

##Mandatory Endpoints
The following endpoints must be implemented with the same names and paths:

GET /products: Listing of products.
POST /products: Creating a product.
GET /sales: Listing of sales
POST /sales: Creating a sale with application of tax rules.
DELETE /sales/{id}: cancel the sales id XXX

baseUrl= http://ocelot-gateway:7777/

Endpoints should return standardized responses:
{
  "data": [{}],
  "status": "success",
  "message": "Operation success concluded "
}

##Docker :
You will receive the docker-compose and both Dockerfile, you can you or not. If you decide to not use, please follow this details below.

You should use the version 3.3 in your docker-compose

some importants points to make all test process flows are below, pay attention in those points to include in your docker-compose:

services:
  gateway:
    container_name: ocelot-gateway
    build:
      context: ./src
...

  api:
    container_name: sales-api
    build:
      context: ./src  # Diretorio onde o codigo da API esta localizado
...

networks:
  evaluation-network:   <- you should use this network name and make this externally.
    external: true
	
....

dont use port 8080 because this is the Jenkins port and will cause conflict.
			
If the repository is structured correctly, docker-compose will identify each Dockerfile and build the images automatically.
You should follow the conteiner names due the tests will call the endpoints.

Checklist for the candidate:

1. The repository must contain:
	src/SalesApi/Dockerfile (Sales API)
	src/Gateway/Dockerfile (Ocelot Gateway)
	docker-compose.yml in the root of the project.
	
docker-compose.yml must be configured to point to the correct Dockerfiles.
 
## Project Structure
This section describes the overall structure and organization of the project files and directories.
 
See [Project Structure](/.doc/project-structure.md)
tem menu de contexto