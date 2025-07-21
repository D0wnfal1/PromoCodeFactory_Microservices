# PromoCode Factory - Microservices Architecture

## Project Description
PromoCode Factory is a distributed system for managing and distributing promotional codes. The application follows a microservices architecture and consists of three independent services that work together:

1. **Administration Service**: Manages employees, roles, and tracks promocode usage by employees.
2. **Giving To Customer Service**: Handles customer data, preferences, and distributes promocodes to customers.
3. **Receiving From Partner Service**: Manages partner relationships, enforces promocode limits, and receives promocodes from partners.

The system enables a complete workflow where partners can provide promocodes (with limits), administrators can manage the system, and customers receive promocodes based on their preferences.

## System Architecture

### Microservices
The system consists of three main microservices:

#### 1. Administration Service
- **Functionality**: 
  - Employee and role management
  - Tracking applied promocodes per employee
  - Receiving notifications about partner manager promocode distributions
- **Database**: PostgreSQL
- **Port**: 5583
- **Additional Services**: Redis (caching)

#### 2. Giving To Customer Service
- **Functionality**:
  - Customer data management
  - Customer preference tracking
  - Promocode assignment to customers based on preferences
  - Notification services for promocode distributions
- **Database**: PostgreSQL
- **Port**: 5582

#### 3. Receiving From Partner Service
- **Functionality**:
  - Partner relationship management
  - Partner promocode limit enforcement
  - Promocode validation and processing
  - Integration with other services to distribute promocodes
- **Database**: MongoDB
- **Port**: 5581

### Communication
- **Synchronous**: HTTP API calls between services
- **Asynchronous**: RabbitMQ message broker for event-driven communication

## Technologies Used
- **.NET 6+**: All microservices are built using .NET 6 or later
- **ASP.NET Core Web API**: RESTful API implementation
- **Entity Framework Core**: ORM for PostgreSQL databases
- **MongoDB Driver**: For the Receiving From Partner service
- **PostgreSQL**: Relational database for Administration and Giving To Customer services
- **MongoDB**: NoSQL database for the Receiving From Partner service
- **Redis**: Caching layer for the Administration service
- **RabbitMQ**: Message broker for asynchronous communication
- **Docker & Docker Compose**: Containerization and orchestration
- **Swagger/OpenAPI**: API documentation

## Project Structure
```
PromoCodeFactory_Microservices/
├── docker-compose.yml                # Docker Compose configuration
└── src/
    ├── Pcf.Administration/           # Administration microservice
    │   ├── Dockerfile
    │   ├── Pcf.Administration.Core/
    │   ├── Pcf.Administration.DataAccess/
    │   ├── Pcf.Administration.IntegrationTests/
    │   └── Pcf.Administration.WebHost/
    │
    ├── Pcf.GivingToCustomer/         # Giving To Customer microservice
    │   ├── Dockerfile
    │   ├── Pcf.GivingToCustomer.Core/
    │   ├── Pcf.GivingToCustomer.DataAccess/
    │   ├── Pcf.GivingToCustomer.Integration/
    │   ├── Pcf.GivingToCustomer.IntegrationTests/
    │   └── Pcf.GivingToCustomer.WebHost/
    │
    └── Pcf.ReceivingFromPartner/     # Receiving From Partner microservice
        ├── Dockerfile
        ├── Pcf.ReceivingFromPartner.Core/
        ├── Pcf.ReceivingFromPartner.DataAccess/
        ├── Pcf.ReceivingFromPartner.Integration/
        ├── Pcf.ReceivingFromPartner.UnitTests/
        └── Pcf.ReceivingFromPartner.WebHost/
```

## Core Features

### Administration Service
- Employee management (CRUD operations)
- Role management and assignment
- Employee promocode application tracking
- Caching of frequently accessed data using Redis
- Integration with RabbitMQ for notifications

### Giving To Customer Service
- Customer management
- Customer preference tracking
- Promocode distribution to customers based on preferences
- Integration with notification services

### Receiving From Partner Service
- Partner management
- Partner promocode limit setting and enforcement
- Promocode validation and processing
- Integration with other services to distribute promocodes
- Notification systems for partners and administrators

## How to Run the Project

### Prerequisites
- Docker and Docker Compose installed
- .NET 6 SDK (for running without Docker)

### Using Docker (Recommended)
The entire system can be launched with a single command:

```bash
# From the root directory
docker-compose up -d
```

This will start all services, databases, Redis, and RabbitMQ.

Service endpoints:
- Administration API: http://localhost:5583
- Giving To Customer API: http://localhost:5582
- Receiving From Partner API: http://localhost:5581
- RabbitMQ Management: http://localhost:15672 (guest/guest)

### Using CLI
Each service can also be run individually:

```bash
# For Administration service
cd src/Pcf.Administration/Pcf.Administration.WebHost
dotnet run

# For Giving To Customer service
cd src/Pcf.GivingToCustomer/Pcf.GivingToCustomer.WebHost
dotnet run

# For Receiving From Partner service
cd src/Pcf.ReceivingFromPartner/Pcf.ReceivingFromPartner.WebHost
dotnet run
```

Make sure to configure the appropriate connection strings in each service's appsettings.json file if running without Docker.

## API Documentation
Each service has its own Swagger documentation available when the service is running:
- Administration: http://localhost:5583/swagger
- Giving To Customer: http://localhost:5582/swagger
- Receiving From Partner: http://localhost:5581/swagger

## Data Flow
1. **Partner Workflow**:
   - Partner submits promocodes through the Receiving From Partner service
   - Service validates against partner promocode limits
   - Valid promocodes are forwarded to the Giving To Customer service
   - Administrators are notified via the Administration service

2. **Customer Workflow**:
   - Customer preferences are stored in the Giving To Customer service
   - When matching promocodes are received, they are assigned to customers
   - Customers are notified about their new promocodes

3. **Administration Workflow**:
   - Administrators manage employees and roles
   - Track employee performance related to promocode usage 