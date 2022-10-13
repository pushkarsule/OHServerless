# This function is not intended to be invoked directly. Instead it will be
# triggered by an orchestrator function.
# Before running this sample, please:
# - create a Durable orchestration function
# - create a Durable HTTP starter function
# - add azure-functions-durable to requirements.txt
# - run pip install -r requirements.txt

import logging
import os
import pathlib
from azure.storage.blob import BlobServiceClient
from azure.core.exceptions import ResourceExistsError

connect_str = os.getenv('ch6fileproc_STORAGE')

def main(name: str) -> str:
        # Create the BlobServiceClient object which will be used to create a container client
    blob_service_client = BlobServiceClient.from_connection_string(connect_str)
    
    # Create a unique name for the container
    container_name = "filestor"
    
    # Create the container if it does not exist
    try:
        blob_service_client.create_container(container_name)
    except ResourceExistsError:
        pass

    # Create a blob client using the local file name as the name for the blob
    blob_name = name
    blob_client = blob_service_client.get_blob_client(container=container_name, blob=blob_name)

    return f"Hello {name}!"
