"""Poll App Views.
"""

from django.db.models import Count, F
from django.http import HttpResponseRedirect
from django.shortcuts import get_object_or_404, render
from django.urls import reverse
from django.utils import timezone
from django.views import generic

from .models import Choice, Question

class IndexView(generic.ListView):
    """Index view."""
    template_name = 'polls/index.html'
    context_object_name = 'latest_question_list'

    def get_queryset(self):
        return Question.objects.filter(
            pub_date__lte=timezone.now()
        ).annotate(
            num_choices=Count('choice')
        ).filter(
            num_choices__gt=0
        ).order_by('-pub_date')[:5]


class DetailView(generic.DetailView):
    """Detail view."""
    model = Question
    template_name = 'polls/detail.html'

    def get_queryset(self):
        """
        Excludes any questions that aren't published yet or no choice.
        """
        return Question.objects.filter(
            pub_date__lte=timezone.now()
        ).annotate(
            num_choices=Count('choice')
        ).filter(
            num_choices__gt=0
        )


class ResultsView(generic.DetailView):
    """Result view."""
    model = Question
    template_name = 'polls/results.html'

    def get_queryset(self):
        """
        Excludes any questions that aren't published yet or no chice.
        """
        return Question.objects.filter(
            pub_date__lte=timezone.now()
        ).annotate(
            num_choices=Count('choice')
        ).filter(
            num_choices__gt=0
        )


def vote(request, question_id):
    '''Vote view.'''
    question = get_object_or_404(Question, pk=question_id)
    try:
        selected_choice = question.choice_set.get(pk=request.POST['choice'])
    except (KeyError, Choice.DoesNotExist):
        return render(request, 'polls/detail.html', {
            'question': question,
            'error_message': "You didn't select a choice.",
        })
    else:
        # Use F() to avoid race condition.
        selected_choice.votes = F('votes') + 1
        selected_choice.save()
        return HttpResponseRedirect(reverse('polls:results', args=(question.id,)))
