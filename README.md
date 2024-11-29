
# ShoppingCart Demo Repository

This repository contains a sample **Shopping Cart** application built with **.NET**, showcasing an end-to-end demonstration of **Snyk's security scanning capabilities**, including:

- **SAST (Static Application Security Testing)** for identifying vulnerabilities in code.
- **Open Source Dependency Scanning** for identifying vulnerabilities in third-party libraries.
- **Container Scanning** for securing Docker images.

## Table of Contents

- [Overview](#overview)
- [Features](#features)
- [Getting Started](#getting-started)
- [How Snyk is Integrated](#how-snyk-is-integrated)
  - [Snyk SAST](#snyk-sast)
  - [Snyk Open Source](#snyk-open-source)
  - [Snyk Container](#snyk-container)
- [Running the Scans Locally](#running-the-scans-locally)
- [Continuous Integration Setup](#continuous-integration-setup)
- [Generated Reports](#generated-reports)
- [Resources](#resources)

---

## Overview

This repository includes the following key components:

- **ShoppingCartApi**: A backend API for managing shopping cart operations.
- **ShoppingCartWeb**: A web application for interacting with the shopping cart.
- **ProductApi**: A backend API for managing product-related operations.
- **IaC (Infrastructure as Code)**: YAML files for deploying the application on Kubernetes.

This project demonstrates how Snyk can be integrated into the development lifecycle to identify and fix security vulnerabilities.

---

## Features

1. **Code Scanning**:
   - Detect vulnerabilities in the application code using **Snyk SAST**.
2. **Dependency Scanning**:
   - Identify and fix vulnerabilities in third-party libraries with **Snyk Open Source**.
3. **Container Scanning**:
   - Analyze Docker images for vulnerabilities using **Snyk Container**.

---

## Getting Started

1. Clone the repository:
   ```bash
   git clone https://github.com/PradeepLoganathan/ShoppingCart.git
   cd ShoppingCart
   ```

2. Install .NET SDK (if not already installed):
   ```bash
   sudo apt-get install dotnet-sdk-8.0
   ```

3. Restore the dependencies for all projects:
   ```bash
   dotnet restore ShoppingCart.sln
   ```

4. Build and run the application:
   ```bash
   dotnet build ShoppingCart.sln
   dotnet run --project ShoppingCartWeb/ShoppingCartWeb.csproj
   ```

---

## How Snyk is Integrated

### Snyk SAST
- SAST scans are run during the CI pipeline to detect vulnerabilities in the application code.
- Results are exported as **SARIF** files and uploaded to GitHub's Code Scanning interface for visualization.

### Snyk Open Source
- Scans the `csproj` files for third-party dependencies and identifies known vulnerabilities in those libraries.
- Configured to block builds if high-severity vulnerabilities are detected.

### Snyk Container
- Scans Docker images built from the `Dockerfile` in each project to identify vulnerabilities in base images and layers.
- Results are pushed to the Snyk dashboard for monitoring.

---

## Running the Scans Locally

### Install Snyk CLI
1. Install Snyk CLI:
   ```bash
   npm install -g snyk
   ```

2. Authenticate with Snyk:
   ```bash
   snyk auth
   ```

### SAST Scan
Run SAST scan for a project:
```bash
snyk code test --severity-threshold=low --sarif-file-output=results.sarif
```

### Open Source Scan
Scan dependencies for vulnerabilities:
```bash
snyk test --file=ProductApi/ProductApi.csproj --severity-threshold=low
```

### Container Scan
Build and scan the Docker image:
```bash
docker build -t shoppingcart-api:latest -f ShoppingCartApi/Dockerfile .
snyk container test shoppingcart-api:latest
```

---

## Continuous Integration Setup

The repository includes a **GitHub Actions** workflow (`.github/workflows/ci.yaml`) with the following steps:

1. **Snyk SAST**: 
   - Scans application code for vulnerabilities.
   - Generates SARIF reports and uploads them to GitHub's Code Scanning interface.

2. **Snyk Open Source**: 
   - Scans project dependencies for known vulnerabilities.
   - Blocks builds for high-severity issues.

3. **Snyk Container**: 
   - Scans Docker images built from the `Dockerfile` for vulnerabilities.
   - Pushes results to the Snyk dashboard.

To trigger the CI workflow:
- Push to the `main` branch.
- Open a pull request targeting `main`.

---

## Generated Reports

- **SARIF Reports**: Stored in project directories (e.g., `ShoppingCartWeb/ShoppingCartWeb.sarif`) and uploaded to GitHub's Security Dashboard.
- **Snyk Dashboard**: View detailed scanning results for SAST, dependencies, and container images.

---

## Resources

- **Snyk Documentation**: [https://docs.snyk.io](https://docs.snyk.io)
- **Snyk CLI Commands**: [https://snyk.io/docs/using-snyk/](https://snyk.io/docs/using-snyk/)
- **GitHub Actions for Snyk**: [Snyk GitHub Actions](https://github.com/snyk/actions)

---
