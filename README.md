<p align="center">
<img width=10% src="ConnectUs/wwwroot/images/connectus-logo.png">onnect-Us
</p>

<p align="center">
  <a href="#-basic-overview">Basic Overview</a> •
  <a href="#-app-screenshots">App Screenshots</a> •
  <a href="#architecture">Architecture</a> •
  <a href="#-docker-services">Docker Services</a> •
  <a href="#development-notes">Development Notes</a> •
  <a href="#-usage">Usage</a> •
  <a href="#-folder-structure">Folder Structure</a> •
  <a href="#limitations">Limitations</a> •
</p>  

## 📌 Basic Overview 

Connect-Us is an e-commerce social media platform that allows creators to showcase products through events, view chat room sentiment of their displayed products, and engage with attendees of events. Shoppers can browse products while participating in events and initiate purchases through Stripe.


## 📷 App Screenshots

Home         |  Event   |    Checkout      |     Dashboard
:-------------------------:|:-------------------------:|:-------------------------:|:-------------------------:
<img src="ConnectUs/wwwroot/images/screenshot4.png" title="Home" width="100%"> |<img src="ConnectUs/wwwroot/images/screenshot3.png" title="Event" width="100%">|<img src="ConnectUs/wwwroot/images/screenshot2.png" title="Checkout" width="100%"> |<img src="ConnectUs/wwwroot/images/screenshot1.png" title="Dashboard" width="100%">


## Architecture
Connect-Us uses ASP.NET Core Razor Pages, following an MVC-based separation of concerns. Razor views (.cshtml) handle the user interface, while PageModels (.cshtml.cs) process requests and prepare data for display. Entity Framework Core manages database access through the application's data models.

## 📤 Docker Services
| Service | Description |
| --- | --- |
| app | Uses the .NET SDK to build and run the ASP.NET Core application in development mode.|
| reverseproxy | Uses Nginx as a reverse proxy, forwarding client requests to the application container and returning its responses. |
| db | Uses Azure SQL Edge to provide a persistent SQL Server database for the application.|
| node | Uses Node.js to provide frontend tooling for compiling Sass and managing Node modules. |

## Development Notes

### Database and migrations
When the development container starts, the startup script automatically runs:
    $ dotnet build
    $ dotnet ef database update

### Environment Variables & Security

- .env stores local database credentials and Stripe API keys

- .env.example provides placeholder values for the required environment variables

- Sensitive config values are passed to the application through Docker Compose environment variables

## 🔎 Usage

### To run on a development environment:
#### On First Start
##### Create .env
    $ cp .env.example .env
##### Configure the required variables in .env, then start the containers
    $ docker compose up -d --build
#### After First Start
##### In subsequent launches, start the existing containers:
    $ docker compose up -d
---

### Useful commands for developers:
#### Start the Node.js Service for Sass Tooling
    $ docker compose up -d node
#### To stop containers
    $ docker compose stop
#### To remove containers
    $ docker compose down
#### To reset the local database
    $ docker compose down -v
    $ docker compose up -d --build
#### If the browser displays **502 Bad Gateway** immediately after the app container is recreated, restart the proxy
    $ docker compose restart reverseproxy

## 📂 Folder Structure

Key pieces of the application are shown below.


```
├── ConnectUs
│   ├── Areas
│   │   ├── Account
│   │   ├── Admin
│   │   ├── Attendee
│   │   ├── Creator
│   │   └── Events
│   ├── ConnectUs.csproj
│   ├── Controllers       
│   ├── Hubs
│   ├── Migrations
│   ├── Model          
│   ├── Program.cs
│   ├── Properties
│   ├── Services
│   ├── Startup.cs
│   ├── appsettings.Development.json
│   ├── appsettings.json
│   ├── docker               
│   │   ├── app
│   │   │   ├── Dockerfile
│   │   │   ├── dev-entrypoint.sh
│   │   │   ├── dev.Dockerfile
│   │   │   └── entrypoint.sh
│   │   ├── nginx
│   │   │   ├── Dockerfile
│   │   │   └── nginx.conf
│   │   └── node
│   │       ├── Dockerfile
│   │       ├── dev.Dockerfile
│   │       └── entrypoint.sh
│   ├── package.json
│   ├── webpack.config.js
├── README.md
├── .env.example
├── .gitignore
├── docker-compose.staging.yml
└── docker-compose.yml
```

## Limitations
Connect-Us targets .NET 5, which is no longer supported, and uses a legacy Azure SQL Edge database image. Production deployment would require a supported .NET and database stack, along with updated dependencies.

This repository is maintained as a demonstration of the original architecture and functionality.

