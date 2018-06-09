"""
Polls Test.
"""

import datetime

from django.test import TestCase
from django.urls import reverse
from django.utils import timezone

from .models import Question

def create_question(question_text, days):
    """
    Create a question.
    """
    time = timezone.now() + datetime.timedelta(days=days)
    question = Question.objects.create(question_text=question_text, pub_date=time)
    return question

def add_choice(question, choice_text):
    """
    Add a choice to a question.
    """
    question.choice_set.create(choice_text=choice_text, votes=0)

class QuestionModelTests(TestCase):
    """
    Test Question model.
    """
    def test_recently_with_future(self):
        """
        was_published_recently() returns False for questions whose pub_date
        is in the future.
        """
        time = timezone.now() + datetime.timedelta(days=30)
        future_question = Question(pub_date=time)
        self.assertIs(future_question.was_published_recently(), False)

    def test_recently_with_old_question(self):
        """
        was_published_recently() returns False for questions whose pub_date
        is older than 1 day.
        """
        time = timezone.now() - datetime.timedelta(days=1, seconds=1)
        old_question = Question(pub_date=time)
        self.assertIs(old_question.was_published_recently(), False)

    def test_recently_with_recent(self):
        """
        was_published_recently() returns True for questions whose pub_date
        is within the last day.
        """
        time = timezone.now() - datetime.timedelta(hours=23, minutes=59, seconds=59)
        recent_question = Question(pub_date=time)
        self.assertIs(recent_question.was_published_recently(), True)

class QuestionIndexViewTests(TestCase):
    """
    Test Question IndexView.
    """
    def test_no_questions(self):
        """
        If no questions exist, an appropriate message is displayed.
        """
        response = self.client.get(reverse('polls:index'))
        self.assertEqual(response.status_code, 200)
        self.assertContains(response, "No polls are available.")
        self.assertQuerysetEqual(response.context['latest_question_list'], [])

    def test_past_question(self):
        """
        Questions with a pub_date in the past are displayed on the
        index page.
        """
        question = create_question(question_text="Past question.", days=-30)
        add_choice(question, 'choice 1')
        response = self.client.get(reverse('polls:index'))
        self.assertQuerysetEqual(
            response.context['latest_question_list'],
            ['<Question: Past question.>']
        )

    def test_future_question(self):
        """
        Questions with a pub_date in the future aren't displayed on
        the index page.
        """
        question = create_question(question_text="Future question.", days=30)
        add_choice(question, 'choice 1')
        response = self.client.get(reverse('polls:index'))
        self.assertContains(response, "No polls are available.")
        self.assertQuerysetEqual(response.context['latest_question_list'], [])

    def test_future_question_and_past(self):
        """
        Even if both past and future questions exist, only past questions
        are displayed.
        """
        past_question = create_question(question_text="Past question.", days=-30)
        add_choice(past_question, 'choice 1')
        future_question = create_question(question_text="Future question.", days=30)
        add_choice(future_question, 'choice 1')
        response = self.client.get(reverse('polls:index'))
        self.assertQuerysetEqual(
            response.context['latest_question_list'],
            ['<Question: Past question.>']
        )

    def test_two_past_questions(self):
        """
        The questions index page may display multiple questions.
        """
        past_question1 = create_question(question_text="Past question 1.", days=-30)
        add_choice(past_question1, 'choice 1')
        past_question2 = create_question(question_text="Past question 2.", days=-5)
        add_choice(past_question2, 'choice 1')
        response = self.client.get(reverse('polls:index'))
        self.assertQuerysetEqual(
            response.context['latest_question_list'],
            ['<Question: Past question 2.>', '<Question: Past question 1.>']
        )

    def test_no_choice_question_not_pub(self):
        """
        If no choice exist, the quesiton not published.
        """
        create_question(question_text="Past question 1.", days=-5)
        question2 = create_question(question_text="Past question 2.", days=-5)
        add_choice(question2, 'choice 1')
        response = self.client.get(reverse('polls:index'))
        self.assertQuerysetEqual(
            response.context['latest_question_list'],
            ['<Question: Past question 2.>']
        )


class QuestionDetailViewTests(TestCase):
    """
    Test QuestionDetailView.
    """
    def test_future_question(self):
        """
        The detail view of a question with a pub_date in the future
        returns a 404 not found.
        """
        future_question = create_question(question_text='Future question.', days=5)
        add_choice(future_question, 'choice 1')
        url = reverse('polls:detail', args=(future_question.id,))
        response = self.client.get(url)
        self.assertEqual(response.status_code, 404)

    def test_past_question(self):
        """
        The detail view of a question with a pub_date in the past
        displays the question's text.
        """
        past_question = create_question(question_text='Past Question.', days=-5)
        add_choice(past_question, 'choice 1')
        url = reverse('polls:detail', args=(past_question.id,))
        response = self.client.get(url)
        self.assertContains(response, past_question.question_text)

    def test_skip_no_choice_question(self):
        """
        The detail view of a question with a pub_date in the future
        returns a 404 not found.
        """
        question = create_question(question_text='Future question.', days=-5)
        url = reverse('polls:detail', args=(question.id,))
        response = self.client.get(url)
        self.assertEqual(response.status_code, 404)
        add_choice(question, 'choice 1')
        response = self.client.get(url)
        self.assertContains(response, question.question_text)
