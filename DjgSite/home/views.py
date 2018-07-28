from django.http import HttpResponse
from django.shortcuts import render

import myutils

# Create your views here.
def index(request):
    database=  myutils.query_config('database')
    return HttpResponse('hello world, you\'re at the django site')