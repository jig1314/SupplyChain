# Supply Chain Management Application
This application is a demo to showcase design patterns related to microservice architecture for a term project for SWE 6853 (Summer 2022). This repository contains two web applications:  
  1. A warehouse management application that allows its users to manage warehouse and receive and ship items. (https://warehouse-mgmt.azurewebsites.net/)
  2. A inventory management application that allows its users to view inventory in the warehouses. (https://inventory-mgr.azurewebsites.net/)

See more information about the design patterns utilized to implement this application below.

## Microservice Architecture Design Patterns
The microservice architecture is an architecutre that builds an application by breaking it down into a set of loosely coupled services (Microservices Pattern: Microservice architecture pattern). Each service can function independently and handles its own responsiblilities. See diagram below:

<p align="center">
  <img src="https://user-images.githubusercontent.com/10623036/181138047-1a22ba83-ab59-47b6-bbf1-f2127afaf5ab.png">
</p>

The benefits of using this Architecture Design Pattern are:
* It improves the maintainability, testability, fault isolation and deployability of the application as a whole.
  *  Each service can be developed, tested and deployed on its own.
  *  The independence makes it so when an issue occurs with own service the others can function as normal.
* Enables software development teams to be organized in a manner that makes them autonomous.

The consequences of using this pattern are:
* It increases the complexity of developing the system as a whole.
* Developer tool support for microservices isn't as robust as for monolithic applications.
* Implementing and testing transactions that span multiple services is difficult.

Using this architecture design pattern requires solving problems like the following:
* How to partition the system into microservices?
* How to ensure the services are independent?
* How to ensure data consistency between services?

The supply chain management application solves these problems by implementing the patterns described below.

### Subdomain decomposition
To answer the issue of system partitioning, the decision was made to use the Decompose by subdomain design pattern. This means defining services to corresponding Domain-Driven Design (DDD) subdomains. DDD refers to the application’s problem space, the business, as the domain (Microservices pattern: Decompose by subdomain). The supply chain management domain is complex and can be easily split into subdomains. See diagram  below:

<p align="center">
  <img src="https://user-images.githubusercontent.com/10623036/181138723-948a1d25-64df-4286-abac-a33792f2e625.png">
</p>

The benefits of this pattern are:
* The architecture is as stable as the domain itself.
* Development teams can be organized around delivering business value of the subdomain rather than technical features.
* Ensures the cohesive yet independent nature of the services.

The consequences of this pattern are:
* Domain knowledge is required in order to identify  subdomains 
* Managing the organizational structure of the development teams corresponding to their subdomains.

### Database per service
To answer the issue of service independence, the decision was made to use the Database per service design pattern. This means each service can control access to the data it needs and its transactions only involve its database. The service's database can't be accessed directly by any other service and its data is only accessible via an API the service exposes (Microservices pattern: Database per service). See diagram  below:

<p align="center">
  <img src="https://user-images.githubusercontent.com/10623036/181138083-38d61e1c-718c-4303-a8ed-1c5390df2ddc.png">
</p>

The benefits of this pattern are:
* Ensure that services are loosely coupled.
* Each service can use the type of database it needs. (maximizes flexibility)

The consequences of this pattern are:
* Implementing queries that join data that is now in multiple databases is challenging.
* Database management is more complex.
* Implementing transactions that span multiple services is much harder. (The next pattern address this concern)

### Saga pattern (Choreography-based)
<p align="center">
  <img src="https://user-images.githubusercontent.com/10623036/181138089-bfb35c8f-24c4-4bf0-aced-162f422f2b97.png">
</p>

#### Transactional outbox pattern
<p align="center">
  <img src="https://user-images.githubusercontent.com/10623036/181138104-a9cf264e-7f3a-47c0-abdb-949e9f20183c.png">
</p>

## Supply Chain Management Application Architecture

<p align="center">
  <img src="https://user-images.githubusercontent.com/10623036/181141998-c5633d74-2bac-4cda-919a-eceb32dfa086.png">
</p>

<p align="center">
  <img src="https://user-images.githubusercontent.com/10623036/181141999-2c7add2c-034f-42c8-8575-95bffc8a983d.png">
</p>

## Work Citied
Microservices pattern: Database per service. microservices.io. (n.d.). Retrieved July 23, 2022, from https://microservices.io/patterns/data/database-per-service.html

Microservices pattern: Decompose by subdomain. microservices.io. (n.d.). Retrieved July 26, 2022, from https://microservices.io/patterns/decomposition/decompose-by-subdomain.html 

Microservices pattern: Domain event. microservices.io. (n.d.). Retrieved July 26, 2022, from https://microservices.io/patterns/data/domain-event.html 

Microservices Pattern: Microservice architecture pattern. microservices.io. (n.d.). Retrieved July 23, 2022, from https://microservices.io/patterns/microservices.html

Microservices pattern: Sagas. microservices.io. (n.d.). Retrieved July 23, 2022, from https://microservices.io/patterns/data/saga.html

Microservices pattern: Transactional outbox. microservices.io. (n.d.). Retrieved July 26, 2022, from https://microservices.io/patterns/data/transactional-outbox.html 
