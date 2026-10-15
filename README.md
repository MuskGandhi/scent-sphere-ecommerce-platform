# ScentSphere Fragrance E-Commerce Platform

## Architecture, Design, and Implementation Report

**Prepared by:** Muskan Gandhi

## 1. Project Overview

ScentSphere is a full-stack fragrance e-commerce platform built with Angular 22, ASP.NET Core 8 Web API, Entity Framework Core, SQL Server, and JWT authentication. It provides customers with a fragrance-shopping experience and gives administrators tools to manage products and inventory.

The solution uses Clean Architecture principles to promote separation of concerns, maintainability, testability, and scalability. It includes authentication, catalog management, cart operations, order processing, audit logging, automated deployment, and code-quality analysis.

## 2. Technology Stack

| Layer or concern | Technology             | Purpose                                                  |
| ---------------- | ---------------------- | -------------------------------------------------------- |
| Frontend         | Angular 22             | Component-based user interface and application structure |
| Backend          | ASP.NET Core 8 Web API | REST API and server-side application logic               |
| ORM              | Entity Framework Core  | Data access, relationship mapping, and migrations        |
| Database         | Microsoft SQL Server   | Relational storage and data consistency                  |
| Authentication   | JWT                    | Stateless authentication for API access                  |
| Testing          | NUnit and Moq          | Unit and integration testing                             |
| Deployment       | Render                 | Hosting and automated deployment from GitHub             |
| Code quality     | SonarCloud             | Static analysis and quality tracking                     |

## 3. Problem Statement and Goals

Customers need to register and sign in, discover fragrances, manage a cart, and place and track orders. Administrators need a secure way to maintain the product catalog and inventory. The platform addresses these needs while emphasizing security, reliability, and maintainability.

### Functional goals

- User registration and authentication
- Product discovery and product details
- Shopping cart management
- Checkout and order history
- Administrative product and inventory management

### Non-functional goals

- Scalability
- Maintainability
- Security
- Reliability
- Testability
- Production readiness

## 4. Requirements

### Customer requirements

- Register and sign in
- Access protected resources
- Browse fragrances and view product details
- Explore products by category and brand
- Add products to a cart, change quantities, and remove products
- Complete checkout and view order history and order status

### Administrative requirements

- Create and update products
- Delete products
- Maintain product inventory
- Access administrative operations securely

## 5. Architecture

### Clean Architecture

The solution separates presentation, application logic, domain rules, infrastructure, and persistence. This limits coupling to frameworks and supports focused testing and independent evolution of the layers.

### Layers and responsibilities

| Layer                        | Responsibilities                                                          |
| ---------------------------- | ------------------------------------------------------------------------- |
| `ScentSphere.Api`            | Controllers, filters, middleware, and dependency injection                |
| `ScentSphere.Application`    | DTOs, interfaces, validators, and application services                    |
| `ScentSphere.Domain`         | Core business entities and enumerations                                   |
| `ScentSphere.Infrastructure` | Authentication, logging, and cross-cutting utilities                      |
| `ScentSphere.Persistence`    | EF Core `DbContext`, repositories, migrations, and database configuration |
| Angular frontend             | User-facing storefront and administrative product-management interface    |

The Angular frontend communicates with the ASP.NET Core API through REST endpoints. Application services encapsulate business operations, and the persistence layer uses Entity Framework Core to interact with SQL Server.

### API controllers

The presentation layer includes controllers for authentication, products, brands, categories, cart operations, orders, and administrative product management.

## 6. Database Design and Integration

SQL Server is the primary relational database, with Entity Framework Core providing data access and migration management. The data model uses relationships and constraints to support consistency and referential integrity.

Entity Framework Core is used for:

- Relationship mapping through navigation properties
- Change tracking
- LINQ queries
- Transaction management
- Repository integration
- Database migrations

### Core entities

- **User:** user information, authentication details, role, and last-login information
- **Product:** name, description, price, stock quantity, brand, and category
- **Shopping cart:** cart and cart-item data for products selected before checkout
- **Order:** order and order-item data, status, and checkout details
- **Supporting entities:** role, brand, category, product image, verification token, and audit log

### Concurrency control

Product updates use a row-version timestamp to detect conflicting concurrent changes:

```csharp
[Timestamp]
public byte[] RowVersion { get; set; }
```

This helps prevent simultaneous updates from silently overwriting one another.

## 7. REST API

The API exposes REST endpoints for authentication, catalog discovery, cart operations, order processing, and product administration.

| Area                   | Operations                                                                          |
| ---------------------- | ----------------------------------------------------------------------------------- |
| Authentication         | `POST /api/auth/register`, `POST /api/auth/login`                                   |
| Products               | `GET /api/products`                                                                 |
| Categories             | `GET /api/categories`                                                               |
| Cart                   | `GET /api/cart`, `POST /api/cart`, `PUT /api/cart`, `DELETE /api/cart`              |
| Orders                 | `POST /api/orders/checkout`, `GET /api/orders`                                      |
| Product administration | `POST /api/admin/products`, `PUT /api/admin/products`, `DELETE /api/admin/products` |

## 8. Security and Request Validation

JWT bearer authentication secures API access. Role-based authorization limits administrative product operations to administrators.

The authentication flow is:

1. A user submits login credentials.
2. The API validates the credentials and issues a JWT.
3. The client stores the token.
4. Authorized requests include the token.

`ValidateModelAttribute` validates incoming request models using their data annotations before controller actions execute. `ExceptionHandlingMiddleware` centralizes handling of unhandled exceptions and returns standardized API responses.

## 9. Business Functionality

### Customer workflows

- Account registration and sign-in
- Product browsing and product-detail viewing
- Cart management
- Checkout and order placement
- Order-history viewing

### Administrative workflows

- Product creation, updates, and deletion
- Catalog and inventory maintenance

Inventory is checked and updated as part of order processing.

## 10. Testing Strategy

The solution includes unit and integration test projects. NUnit is used as the test framework, Moq supports dependency mocking, and the EF Core InMemory provider supports data-oriented tests.

### Unit tests

| Test suite                  | Coverage                                                        |
| --------------------------- | --------------------------------------------------------------- |
| `AuthServiceTests`          | Registration, login, JWT creation, and password verification    |
| `ProductServiceTests`       | Product creation, retrieval, and search                         |
| `CartServiceTests`          | Adding items, retrieving cart contents, and quantity management |
| `OrderServiceTests`         | Checkout, order creation, and inventory reduction               |
| `CategoriesControllerTests` | Category retrieval                                              |

### Integration tests

- `ProductsIntegrationTests`: `GET /api/products`
- `CategoriesIntegrationTests`: `GET /api/categories`
- `BrandsIntegrationTests`: `GET /api/brands`

Unit tests validate service and controller behavior. Integration tests validate API behavior across the application boundary.

## 11. CI/CD and Deployment

The backend is connected to GitHub and deployed on Render. Render monitors the configured branch and starts a deployment when it detects a new commit.

### Deployment flow

```text
Developer changes
    -> Local validation and tests
    -> Git commit and push
    -> GitHub repository update
    -> Render detects the commit
    -> Restore, build, and publish
    -> Application deployment
    -> Production verification
```

The documented .NET build steps are `dotnet restore`, `dotnet build`, and `dotnet publish`. Before pushing a change, the workflow calls for local functionality checks and unit and integration test execution. Build and deployment logs are used to review compilation, publishing, startup, and runtime initialization.

**Production API:** <https://scentsphere-api.onrender.com>

## 12. Code Quality

SonarCloud is used to track maintainability, reliability, security, technical debt, duplicate code, and code smells. Findings are reviewed and addressed through focused changes such as improving exception handling, logging, and removing unnecessary code.

Centralized exception handling and logging improve operational supportability.

## 13. Assumptions and limitations

- **CORS:** The API currently allows browser requests from `http://localhost:4200` only. Add the actual deployed frontend origin to the API CORS policy before using the production frontend from a browser.
- **Database migrations:** Migrations are present but are not applied automatically during API startup. Apply them explicitly to the target database before using the API.
- **Admin route:** The login component navigates administrators to `/admin/product-management`, but the Angular route table declares `/admin/products`. These paths should be reconciled; this report does not change application behavior.
- **Frontend route protection:** `authGuard` exists. The backend still authorizes cart and order APIs, but the frontend may render those routes before an API request is rejected.
- **Token storage:** The frontend stores the JWT in `localStorage`. This is convenient for the current interceptor but makes the token accessible to JavaScript; production hardening should include XSS controls or a reviewed alternative token-storage design.
- **Deployment automation:** The Render URL and Dockerfile show deployment intent.
- **Documentation drift:** The root README names Azure DevOps, while the production environment points to Render and the supplied report describes GitHub-connected Render deployment. Confirm the actual source-control and deployment configuration and update the repository documentation accordingly.
