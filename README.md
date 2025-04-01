# D2LOffice

This is a simple app written using MAUI and Blazor to simplify course creation.  It does it in a way I like.  May not be your cup of tea.

1.  Everything is stored in a weekly checklists.  Students can browse weekly checklists to see what's due, what topics are coming up.  These can be divided into sections, i.e. "Lecture" and "Lab"

2.  You can designate a checklist item as a topic, reminder, assignment, or quiz.  Topics go into an automatically generated webpage calendar that students can glance at.  Reminders get due-dates and are placed into the D2L calendar in addition to the checklist.  Assignments must be assigned a points number and go into checklists, calendar, and a unique assignment is created for them.  Quizzes are buggy still, but the intention is to put them into the checklist with a link to the quiz.  These would require a quiz that already exists, but this feature doesn't work yet.

3.  You can export and import CSV rather than working directly into the app.  An Id column will be visible on the CSV.  Don't touch it as this number is hidden in all of the descriptions to match and make syncing with the app and D2L more seamless.  Once you've synced to D2L once, you want to re-export the CSV (if you intend to edit with CSV) so that the Id column is populated with current items.

Why is this necessary?  I wanted something where I could change a few dates in ONE location... and it would change it everywhere automatically.  It's a lot easier to edit all of the fields at once rather than having to edit each checklist separately.

While I use D2L myself, there's no reason this couldn't work with other LMS's.  While it's not currently setup as a generic interface, that's something that I tried to keep in mind when designing the app. 

![image](https://github.com/user-attachments/assets/f5d22f10-c29f-446e-b687-a31c39de3c52)

![image](https://github.com/user-attachments/assets/11fd7a1e-43d5-4db0-af1c-385bb622f78e)
