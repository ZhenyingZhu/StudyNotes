"""Polls Admin
"""

from django.contrib import admin

from .models import Choice, Question

class QuestionAdmin(admin.ModelAdmin):
    """Define the order of fields."""
    #fields = ['pub_date', 'question_text']
    fieldsets = [
        (None, {'fields': ['question_text']}),
        ('Date information', {'fields': ['pub_date']}),
    ]

admin.site.register(Question, QuestionAdmin)
#admin.site.register(Question, QuestionAdmin)

admin.site.register(Choice)
