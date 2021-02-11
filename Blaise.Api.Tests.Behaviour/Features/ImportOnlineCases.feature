@onlinedata
Feature: Import online cases
	In order to process cases gathered online
	As a service
	I want to be given cases to import representing the data captured online

@smoke
#Scenario 1 https://collaborate2.ons.gov.uk/confluence/display/QSS/OPN+NISRA+Case+Processing+Scenarios
Scenario: A case in the online file is complete and in Blaise it is complete, we take the online case
	Given there is a online file that contains a case that is complete
	And the same case exists in Blaise that is complete
	When the online file is imported
	Then the existing blaise case is overwritten with the online case


#Scenario 2 https://collaborate2.ons.gov.uk/confluence/display/QSS/OPN+NISRA+Case+Processing+Scenarios
Scenario:  A case in the online file is partially complete and in Blaise it is complete, we keep the existing blaise case
	Given there is a online file that contains a case that is partially complete
	And the same case exists in Blaise that is complete
	When the online file is imported
	Then the existing blaise case is kept


#Scenario 3 https://collaborate2.ons.gov.uk/confluence/display/QSS/OPN+NISRA+Case+Processing+Scenarios
Scenario: A case in the online file is complete and in Blaise it is partially complete, we take the online case
	Given there is a online file that contains a case that is complete
	And the same case exists in Blaise that is partially complete
	When the online file is imported
	Then the existing blaise case is overwritten with the online case

#Scenario 4 https://collaborate2.ons.gov.uk/confluence/display/QSS/OPN+NISRA+Case+Processing+Scenarios
Scenario Outline: A case in the online file is complete and in Blaise it is between the range 210-542, we take the online case
	Given there is a online file that contains a case that is complete
	And the same case exists in Blaise with the outcome code '<existingOutcome>'
	When the online file is imported
	Then the existing blaise case is overwritten with the online case
	Examples: 
	| existingOutcome | description                                                 |
	| 210             | Partially completed survey                                  |
	| 310             | Non-contact                                                 |
	| 430             | HQ refusal                                                  |
	| 440             | Person not available                                        |
	| 460             | Refuses cooperation - hard refusal                          |
	| 461             | Refuses cooperation - soft refusal could be contacted again |
	| 541             | Language difficulties - notified by Head Office             |
	| 542             | Language difficulties - notified to interviewer             |


#Scenario 5 https://collaborate2.ons.gov.uk/confluence/display/QSS/OPN+NISRA+Case+Processing+Scenarios
Scenario: A case in the online file that has not started and in Blaise it is complete, we keep the existing blaise case
	Given there is a online file that contains a case that has not been started
	And the same case exists in Blaise that is complete
	When the online file is imported
	Then  the existing blaise case is kept


#Scenario 6  https://collaborate2.ons.gov.uk/confluence/display/QSS/OPN+NISRA+Case+Processing+Scenarios
Scenario: A case in the online file is partially complete and in Blaise it is partially complete, we take the online case
	Given there is a online file that contains a case that is partially complete
	And the same case exists in Blaise that is partially complete
	When the online file is imported
	Then the existing blaise case is overwritten with the online case

#Scenario 7  https://collaborate2.ons.gov.uk/confluence/display/QSS/OPN+NISRA+Case+Processing+Scenarios
Scenario Outline: A case in the online file is partially complete and in Blaise and it is between the range 310-542, we take the online case
	Given there is a online file that contains a case that is complete
	And the same case exists in Blaise with the outcome code '<existingOutcome>'
	When the online file is imported
	Then the existing blaise case is overwritten with the online case
	Examples: 
	| existingOutcome | description                                                 |
	| 310             | Non-contact                                                 |
	| 430             | HQ refusal                                                  |
	| 440             | Person not available                                        |
	| 460             | Refuses cooperation - hard refusal                          |
	| 461             | Refuses cooperation - soft refusal could be contacted again |
	| 541             | Language difficulties - notified by Head Office             |
	| 542             | Language difficulties - notified to interviewer             |


#Scenario 8 https://collaborate2.ons.gov.uk/confluence/display/QSS/OPN+NISRA+Case+Processing+Scenarios
Scenario: A case in the online file that has not started and in Blaise it is non-contact, we keep the existing blaise case
	Given there is a online file that contains a case that has not been started
	And the same case exists in Blaise with the outcome code '310'
	When the online file is imported
	Then  the existing blaise case is kept


#Scenario 9 https://collaborate2.ons.gov.uk/confluence/display/QSS/OPN+NISRA+Case+Processing+Scenarios
Scenario: A case in the online file that is partially complete and in Blaise it marked as respondent request for data to be deleted, we keep the existing blaise case
	Given there is a online file that contains a case that is partially complete
	And the same case exists in Blaise with the outcome code '562'
	When the online file is imported
	Then  the existing blaise case is kept


#Scenario 10 https://collaborate2.ons.gov.uk/confluence/display/QSS/OPN+NISRA+Case+Processing+Scenarios
Scenario: A case in the online file that is complete and in Blaise it marked as respondent request for data to be deleted, we keep the existing blaise case
	Given there is a online file that contains a case that is complete
	And the same case exists in Blaise with the outcome code '561'
	When the online file is imported
	Then  the existing blaise case is kept

#Additional NFR Scenarios 

Scenario: There is a no online file available and Blaise contains no cases
	Given there is a not a online file available 
	And blaise contains no cases
	When the online file is imported
	Then blaise will contain no cases

Scenario: There is a no online file available and Blaise contains 10 cases
	Given there is a not a online file available 
	And blaise contains '10' cases
	When the online file is imported
	Then blaise will contain '10' cases

Scenario: There is an online file available with 10 cases and Blaise contains no cases
	Given there is a online file that contains '10' cases 
	And blaise contains no cases
	When the online file is imported
	Then blaise will contain '10' cases