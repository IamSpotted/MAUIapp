# ITSF App - MAUI Project

## Overview

The **ITSF App** is a cross-platform mobile application built using **.NET MAUI**. The app leverages various libraries such as **CommunityToolkit.Maui**, **Syncfusion**, and **Microsoft.Extensions.Logging** for improved functionality and user experience. This app is designed to manage and interact with **Projects**, **Tasks**, and **Tags**, allowing users to easily organize and monitor their work.

## Features

- Manage Projects and Tasks
- Create and edit Tags for categorizing tasks
- Seamless integration with Syncfusion controls for advanced UI elements
- Error handling with modal alerts for better user experience
- Logging and debugging support

## Tech Stack

- **.NET MAUI** - Cross-platform UI framework
- **CommunityToolkit.Maui** - For utility helpers and UI controls
- **Syncfusion** - For advanced UI elements like charts, data grids, etc.
- **SQLite** - Local database for storing app data
- **Microsoft.Extensions.Logging** - For logging and debugging

## Installation

To run the **ITSF App** locally, follow these steps:

### Prerequisites

- **.NET 6 or higher** installed
- **Visual Studio 2022 or higher** with the MAUI workload
- **SQLite** for local database storage

### Setup Instructions

1. Clone the repository to your local machine:
   ```bash
   git clone https://github.com/yourusername/ITSF-App.git

2. **Open the project in Visual Studio.**

3. **Restore the NuGet packages:**
   - Right-click on the solution in Solution Explorer.
   - Select **Restore NuGet Packages**.

4. **Run the app:**
   - Select your target platform (Android, iOS, Mac Catalyst, Windows).
   - Press **F5** to build and run the app.

## Core Components

### 1. **MauiProgram.cs**
   - Sets up the MAUI app and configures services like repositories, error handling, and routes.
   - Registers pages with Shell routing.

### 2. **Repositories**
   - **ProjectRepository**: Handles data operations for projects.
   - **TagRepository**: Manages tags and their associations with projects.
   - **TaskRepository**: Handles task data.
   - **CategoryRepository**: Manages categories for projects.

### 3. **PageModels**
   - **ProjectListPageModel**: Handles the logic for displaying the list of projects.
   - **TaskDetailPageModel**: Manages the details of a specific task.
   - **ManageMetaPageModel**: Manages metadata like tags and categories.

### 4. **Models**
   - **Project**: Represents a project with details like name, description, and tasks.
   - **ProjectTask**: Represents a task with properties like title and completion status.
   - **Category**: Represents a category for grouping projects.
   - **Tag**: Represents tags that can be associated with projects or tasks.

## Future Plans

The app is being developed as a skeleton based on the template, with plans for significant changes and additional features. Below are the key features and functionalities planned for future development:

### Links Page
A dedicated **Links page** will be added to the app. This page will allow users to access various web interfaces within the app itself:
- Printer web interfaces
- Camera web interfaces

### Forms Page
A **Forms page** will be introduced, where users can:
- Fill out various forms.
- Print the forms.
- Save the forms to a file share.

### Chat Functionality
The app will allow **multiple users to interact with the app** at the same time, with a potential **chat functionality**. The app will be designed to:
- Use **SignalR** for real-time communication between users.
- Ensure secure, group-based messaging for authenticated users.

### User Authentication and Data Security
The app will integrate **authentication and encryption** from the beginning. Features include:
- **Authentication via Active Directory (AD)** to manage user access.
- **Group-based restrictions** to limit access to chat functionality.
- **Message encryption** to ensure chat messages are secure both in transit and at rest.

### Persistence with SQL Server
The app will use **SQL Server** for data persistence, ensuring reliable storage of:
- User data
- Chat messages
- Forms and other relevant data

The chat messages will be securely stored in the database, encrypted for safety.

### Project Page and Template Cleanup
The current pages from the template (Project List, Task Detail, etc.) will be removed and replaced with the new functionality:
- The **Links page**, **Forms page**, and **Chat page** will be the focus.
- Additional pages will be added one at a time as the app's functionality expands.

---

These plans aim to make the app more functional, user-friendly, and secure, with room for expansion if the app needs to be scaled in the future.

## Contributing

1. Fork the repository.
2. Create a new branch.
3. Make your changes.
4. Submit a pull request.

## Acknowledgments

- **.NET MAUI** team for creating a unified cross-platform framework.
- **Syncfusion** for providing amazing UI components.
- **CommunityToolkit.Maui** for useful utilities in the MAUI ecosystem.
