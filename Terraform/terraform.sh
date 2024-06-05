#!/bin/bash

# Set the path to your .env file (modify if needed)
ENV_FILE="../.env"

# Check if the .env file exists
if [[ ! -f "$ENV_FILE" ]]; then
  echo "Error: .env file not found!"
  exit 1
fi

# Read each line in the .env file
while IFS='=' read -r VAR_NAME VAR_VALUE; do
  # Skip empty lines and comments
  if [[ -z "$VAR_NAME" || "$VAR_NAME" =~ ^[[:blank:]]*# ]]; then
    continue
  fi

  # Prefix variable name with TF_VAR_ for Terraform
  TF_VAR_NAME="TF_VAR_$VAR_NAME"

  # Export variable with the parsed value
  export "$TF_VAR_NAME=$VAR_VALUE"
done < <(grep -Ev "^[[:blank:]]*#" "$ENV_FILE")

# Inform user about loaded variables
echo "Loaded variables from .env for Terraform:"
#env | grep ^TF_VAR_


if [[ "$1" == "apply" ]]; then
  # Code to run if the first argument is "apply"
  terraform apply
else
  # Code to run if the first argument is not "apply"
  terraform plan
fi

# ... other Terraform commands
