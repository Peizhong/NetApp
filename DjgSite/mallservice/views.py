from django.http import HttpResponse
from django.shortcuts import render
import json

# Create your views here.
def index(request):
    api = {
        'get':'lalalal',
        'post':'woo'
    }
    return HttpResponse(json.dumps(api))
