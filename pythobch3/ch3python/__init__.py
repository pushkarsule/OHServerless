import logging

import azure.functions as func
import json


def main(req: func.HttpRequest, ratings: func.DocumentList) -> func.HttpResponse:

    if ratings.count == 0:
        return func.HttpResponse(
        'Rating not found',
        status_code=404
    )

    rating_doc = ratings[0]
    rating = {
        "userId": rating_doc['userId'],
        "id": rating_doc['id'],
        "productId": rating_doc['productId'],
        "timestamp": rating_doc['timestamp'],
        "locationName": rating_doc['locationName'],
        "rating": rating_doc['rating'],
        "userNotes": rating_doc['userNotes']
    }

    rating_json = json.dumps(rating)

    return func.HttpResponse(
        rating_json,
        status_code=200,
        mimetype="application/json"
    )
