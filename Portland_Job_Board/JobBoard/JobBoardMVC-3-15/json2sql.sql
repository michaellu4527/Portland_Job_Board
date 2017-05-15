

/* Insert JSON object between apostrophes */
DECLARE @json NVARCHAR(MAX);
SET @json = '[
  {
    "ApplicationLink": "https://www.amazon.jobs/en/jobs/478313",
    "Company": "Amazon",
    "DatePosted": "Posted January  4, 2017",
    "Experience": "",
    "Hours": "",
    "JobID": "Job ID: 478313",
    "JobTitle": "Portland-based SDE III, AWS",
    "LanguagesUsed": "",
    "Location": "US, OR, Portland",
    "Salary": ""
  },
  {
    "JobTitle": "Sr. Cloud Infrastructure Architect - Portland",
    "Experience": "",
    "DatePosted": "Posted March  3, 2016",
    "Location": "US, OR, Portland",
    "Company": "Amazon",
    "Hours": "",
    "JobID": "Job ID: 386061",
    "LanguagesUsed": "",
    "Salary": "",
    "ApplicationLink": "https://www.amazon.jobs/en/jobs/386061"
  }]';

 INSERT INTO Jobs
 SELECT * 
 FROM OPENJSON(@json)
 WITH (ApplicationLink NVARCHAR(MAX),
		Company NVARCHAR(MAX),
		DatePosted NVARCHAR(MAX),
		Experience NVARCHAR(MAX),
		Hours NVARCHAR(MAX),
		JobID NVARCHAR(MAX),
		JobTitle NVARCHAR(MAX),
		LanguagesUsed NVARCHAR(MAX),
		Location NVARCHAR(MAX),
		Salary NVARCHAR(MAX))
