##-----------------------------------##
## Install Snyk CLI                  ##
##-----------------------------------##

npm install -g snyk

snyk auth

##-----------------------------------##
## Basic Vulnerability Scan:         ##
## Focuses on Open Source Security. It analyzes your project's dependencies (libraries, packages) ##
## listed in manifest files like package.json, pom.xml, or .csproj to identify known vulnerabilities. ##
## It checks if any of the versions you're using have reported security flaws. ##
##-----------------------------------##

# can be low|medium|high|critical
snyk test --dev --json --severity-threshold=low --target-framework=net8.0 > scan-results.json

# List all vulnerabilities with basic information
jq '.vulnerabilities[] | {id: .id, title: .title, severity: .severity, packageName: .packageName, version: .version}' scan-results.json

# Filter and list only high-severity vulnerabilities
jq '.vulnerabilities[] | select(.severity == "high") | {id: .id, title: .title, packageName: .packageName, version: .version}' scan-results.json

# List CVE identifiers for each vulnerability
jq '.vulnerabilities[] | {id: .id, cve: .identifiers.CVE[]}' scan-results.json

#List packages with available upgrades to fix vulnerabilities
jq '.remediation.upgrade | to_entries[] | {package: .key, upgradeTo: .value.upgradeTo}' scan-results.json

# Provide a summary of the number of vulnerable paths
jq '.summary' scan-results.json

##----------------------------------------------------------------------##
## Code Scan: SAST                                                      ##
## SARIF stands for Static Analysis Results Interchange Format.         ##
## Generate the Sarif file and view for remediations                    ##
##----------------------------------------------------------------------##

snyk code test --severity-threshold=low --sarif --sarif-file-output=snyk-code-results.sarif 


## container test

snyk container test pradeepl/shoppingcartapi:latest --file=Dockerfile --sbom-file-output=my-image-sbom.json

snyk container test pradeepl/shoppingcartapi:latest --file=Dockerfile --sarif > vulnerabilities.sarif

snyk container test pradeepl/shoppingcartapi:latest --file=Dockerfile --json > vulnerabilities.json



## IaC test

snyk iac test

