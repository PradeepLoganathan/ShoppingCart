##-----------------------------------##
## Install Snyk CLI                  ##
##-----------------------------------##

npm install -g snyk

snyk auth

##-----------------------------------##
# Let’s start by checking for vulnerabilities in our dependencies.
# Using snyk test, we scan the ShoppingCartApi project and identify all known vulnerabilities
# in our open-source libraries.
# Then, we parse the results to understand severity, affected packages, and possible remediations
##-----------------------------------##

# Navigate to the project directory
cd ShoppingCartApi

# Perform a basic vulnerability scan on dependencies
snyk test --dev --json --severity-threshold=low --target-framework=net8.0 > scan-results.json

# Parse and analyze results
jq '.vulnerabilities[] | {id: .id, title: .title, severity: .severity, packageName: .packageName, version: .version}' scan-results.json

# Filter for high-severity vulnerabilities
jq '.vulnerabilities[] | select(.severity == "high") | {id: .id, title: .title, packageName: .packageName, version: .version}' scan-results.json

# List CVEs for each vulnerability
jq '.vulnerabilities[] | {id: .id, cve: .identifiers.CVE[]}' scan-results.json

# Identify available upgrades for vulnerable packages
jq '.remediation.upgrade | to_entries[] | {package: .key, upgradeTo: .value.upgradeTo}' scan-results.json

# Provide a summary
jq '.summary' scan-results.json

##----------------------------------------------------------------------##
# Next, let’s analyze the source code for vulnerabilities using SAST 
# (Static Application Security Testing).
# This scan will identify issues like SQL injection, XSS, or insecure coding patterns.
# The results are output in SARIF format, perfect for integrating with tools like GitHub
##----------------------------------------------------------------------##

# Scan code for vulnerabilities and generate a SARIF file
snyk code test --severity-threshold=low --sarif --sarif-file-output=snyk-code-results.sarif

# (Optional) Use tools like GitHub's SARIF viewer to analyze the results



##----------------------------------------------------------------------##
# Let’s shift focus to securing our containerized application.
# Using snyk container test, we identify vulnerabilities in the Docker image,
# analyze its SBOM (Software Bill of Materials), and export results for further integration.                                                      ##
##----------------------------------------------------------------------##

snyk container test pradeepl/shoppingcartapi:latest --file=./ShoppingCartApi/Dockerfile --sbom-file-output=my-image-sbom.json --json > snyk-container-results.json

jq '.vulnerabilities[] | {id: .id, title: .title, severity: .severity, packageName: .packageName, version: .version}' snyk-container-results.json 


snyk container test pradeepl/shoppingcartapi:latest --file=Dockerfile --sarif > vulnerabilities.sarif

snyk container sbom --format=cyclonedx1.5+json pradeepl/shoppingcartapi:latest | jq

##----------------------------------------------------------------------##
## IaC test                                                             ##
##----------------------------------------------------------------------##

snyk iac test --json --json-file-output=vuln.json


snyk iac test --sarif --sarif-file-output=vuln.sarif

snyk iac test ./ShoppingCart-IaC/deployment-api.yaml 
