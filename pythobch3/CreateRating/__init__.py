import logging
import azure.functions as func
from jsonschema import validate, ValidationError
import requests
import uuid
import time
import json

def main(req: func.HttpRequest, ratings: func.Out[func.Document]) -> func.HttpResponse:
    schema = {
        "type" : "object",
        "properties" : {
            "userId" : {"type" : "string", "minLength": 1},
            "productId" : {"type" : "string", "minLength": 1},
            "locationName" : {"type" : "string"},
            "rating" : {"type" : "number", "maximum": 5, "minimum": 0},
            "userNotes" : {"type" : "string"},
        },
    }

    try:
        req_body = req.get_json()
        validate(instance=req_body, schema=schema)
    except ValueError:
        return func.HttpResponse(
             "Request body is missing",
             status_code=400
        )
    except ValidationError as err:
        return func.HttpResponse(
             f"Invalid request body: {err}",
             status_code=400
        )
    
    getProductUrl = "https://serverlessohproduct.trafficmanager.net/api/GetProduct"
    getUserUrl = "https://serverlessohuser.trafficmanager.net/api/GetUser"

    productIdParams = {'productId': req_body.get('productId')}
    userIdParams = {'userId': req_body.get('userId')}

    productInfo = requests.get(url=getProductUrl, params=productIdParams)
    if productInfo.status_code == 404:
        return func.HttpResponse(
             f"Product Id not found",
             status_code=404
        )

    userInfo = requests.get(url=getUserUrl, params=userIdParams)
    if userInfo.status_code == 404:
        return func.HttpResponse(
             f"User Id not found",
             status_code=404
        )

    req_body["id"] = str(uuid.uuid4())
    req_body["timestamp"] = time.time()
    
    rating_json = json.dumps(req_body)
    ratings.set(func.Document.from_json(rating_json))

    return func.HttpResponse(
        rating_json,
        status_code=200,
        mimetype="application/json"
    )