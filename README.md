# PROG6221Final
# POE Read Me file content
 github link: https://github.com/ZSalie/PROG6221Final.git

 ## Description
 # Chatbot System Overview  

This document provides a structured explanation of the chatbot's architecture and functionality. The chatbot is designed as a conversational assistant specializing in cybersecurity topics, task management, and educational quizzes.  

## Core Functionality  

The system's primary entry point is the `Respond` method, which processes user input through a structured decision pipeline. This method first checks for activity log requests before proceeding to quiz interactions, follow-up prompts, and task management commands. If none of these apply, it analyzes the input for cybersecurity topics or general sentiment before finally resorting to fallback responses when no matches are found.  

Activity logging maintains a record of recent interactions through the `LogAction` method, which stores timestamped descriptions of key events while preserving only the ten most recent entries. Users can retrieve this history using natural language commands like "show activity log" or "what have you done for me."  

## Interactive Features  

The quiz system operates through three coordinated methods. `StartQuiz` initializes the assessment by resetting tracking variables and presenting the first question. As users respond, `HandleQuizResponse` evaluates answers, provides feedback, and progresses through the question sequence. Each question is formatted for clear presentation via `FormatQuestion`, which structures the text and multiple-choice options. The system maintains quiz state through dedicated flags tracking participation status, current question position, and accumulated score.  

For task management, the system processes natural language commands to create, modify, and review tasks. The parser recognizes imperative phrases like "add task" or "complete task," delegating operations to corresponding TaskManager methods. A sophisticated reminder system captures both the reminder content and temporal parameters through a two-step interaction, first storing the reminder message then parsing a subsequent input for the time specification.  

## Natural Language Processing  

Sentiment analysis identifies emotional cues in user input, generating appropriate empathetic or congratulatory responses. The topic detection system scans for cybersecurity keywords, enabling targeted information delivery. When users express concerns about specific topics, the system provides immediate tips followed by offers for expanded explanations.  

The follow-up mechanism engages users who request additional information, serving comprehensive guidance through coordinated methods that retrieve and format detailed responses. Negative responses to these offers generate polite acknowledgments while keeping the conversation open for future inquiries.  

## Supporting Components  

The TaskManager class provides essential task operations including creation, completion, deletion, and listing. Reminder functionality is encapsulated in a nested class structure preserving message content and activation times. Quiz content is managed through immutable question objects containing all necessary presentation and evaluation data.  

Response management is centralized in the ResponseManager class, which serves both concise tips and detailed explanations while maintaining collections of follow-up content and error messages. This structured approach ensures consistent, context-aware interactions throughout all conversation paths.  

The system combines these elements to deliver a responsive, informative, and user-friendly experience for individuals seeking cybersecurity assistance and task management support.
