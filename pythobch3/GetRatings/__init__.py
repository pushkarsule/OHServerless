import logging

import azure.functions as func
import json


def main(req: func.HttpRequest, ratings: func.DocumentList) -> func.HttpResponse:
    if ratings.count == 0:
        return func.HttpResponse(
        'Rating not found',
        status_code=404
    )

    ratings_output = []

    for rating_doc in ratings:
        rating = {
            "userId": rating_doc['userId'],
            "id": rating_doc['id'],
            "productId": rating_doc['productId'],
            "timestamp": rating_doc['timestamp'],
            "locationName": rating_doc['locationName'],
            "rating": rating_doc['rating'],
            "userNotes": rating_doc['userNotes']
        }

        ratings_output.append(rating)

    rating_json = json.dumps(ratings_output)

    return func.HttpResponse(
        rating_json,
        status_code=200,
        mimetype="application/json"
    )
