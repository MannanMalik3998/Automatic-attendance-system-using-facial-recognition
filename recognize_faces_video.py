
# import the necessary packages
from imutils.video import VideoStream
import face_recognition
import argparse
import imutils
import pickle
import time
import cv2
import sys
import csv
import pandas as pd
import datetime
import numpy as np

#************************* Edits ***************************

courseName=sys.argv[1]
# courseName="PIT"
print(courseName)

upper_left = (200, 200)
bottom_right = (400, 400)
presentStudents = list()
camSource = int(sys.argv[2]) # 0 for laptop cam and 1 for external cam
# camSource = 0 # 0 for laptop cam and 1 for external cam

#encodings = 'encodings2.pickle'
encodings = 'E:\Sem7\HCI\ProjAttendanceSystem\HCI\encodings2.pickle'#change this path according to the file path
#************************************************************
try:
	data = pickle.loads(open(encodings, "rb").read())

	vs = VideoStream(src=camSource).start()

	time.sleep(2.0)

	while True:
		frame = vs.read()

		
		
		
		rgb = cv2.cvtColor(frame, cv2.COLOR_BGR2RGB)
		rgb = imutils.resize(frame, width=750)
		r = frame.shape[1] / float(rgb.shape[1])

		boxes = face_recognition.face_locations(rgb,model='hog')
		encodings = face_recognition.face_encodings(rgb, boxes)
		names = []

		#******************** Edits ************************
		height, width, channels = frame.shape
		upper_left = (int(width / 4)+10, int(height / 4)+10)
		bottom_right = (int(width * 3 / 4)+10, int(height * 3 / 4)+10)


		# draw in the image
		cv2.rectangle(frame, upper_left, bottom_right, (86, 252, 200 ), 2)
		cv2.putText(frame,"Press q to close", (300, 450),cv2.FONT_HERSHEY_SIMPLEX, 1, (255, 0, 0), 1)


		#********************************************

		# loop over the facial embeddings
		for encoding in encodings:

			matches = face_recognition.compare_faces(data["encodings"],encoding)
			
			name = "Unknown"

			# check to see if we have found a match
			if True in matches:
				matchedIdxs = [i for (i, b) in enumerate(matches) if b]
				counts = {}

				for i in matchedIdxs:
					name = data["names"][i]
					counts[name] = counts.get(name, 0) + 1

				name = max(counts, key=counts.get)
			
			# update the list of names
			names.append(name)

		# loop over the recognized faces
		for ((top, right, bottom, left), name) in zip(boxes, names):
			# rescale the face coordinates
			top = int(top * r)
			right = int(right * r)
			bottom = int(bottom * r)
			left = int(left * r)

			# draw the predicted face name on the image
			cv2.rectangle(frame, (left, top), (right, bottom),	(0, 255, 0), 2)
			
			y = top - 15 if top - 15 > 15 else top + 15

			cv2.putText(frame, name, (left, y), cv2.FONT_HERSHEY_SIMPLEX,	0.75, (0, 255, 0), 2)

			#**************************************** Edits *****************************************************************

			a,b = upper_left
			c,d = bottom_right

			if(left > a and left < c and top > b and top < d and right < c and bottom < d):#face is in rectangle
				
				# print("\a")
				cv2.rectangle(frame, upper_left, bottom_right, (200, 252, 200 ), 2)
				if(not presentStudents.__contains__(name)and ( not name.__contains__('Unknown')) ):
					presentStudents.append(name)#mark present
				if not name.__contains__('Unknown'):
					cv2.putText(frame, name+"Present", (30, 20),cv2.FONT_HERSHEY_SIMPLEX, 1, (195, 100, 255), 1)

				#*********************************************************************************************************

		cv2.imshow("Automatic Attendance System -> Facial Recognition", frame)
		key = cv2.waitKey(1) & 0xFF

		# if the `q` key was pressed, break from the loop
		if key == ord("q"):
			break

	# do a bit of cleanup
	cv2.destroyAllWindows()
	vs.stop()

	#**************************************** Edits *****************************************************************

	import os
	#print('Total Students:	',(len(next(os.walk('dataset'))[1]))-1)
	# print('Total Students:	',4)
	print(4)

	print(presentStudents.__len__())
	# print("Total Students Present:",presentStudents.__len__())

	for i in presentStudents:
		print(i)
	#All present students displayed

	#Creating CSV*********************************************************8
	
	
	StudentNames = ["Manan","Murtaza","Imtiaz","Yasir"]
	#Adding New Attendance
	def AddAttendance(Attendance, StudentNames, PresentStudents):   
		Date = (datetime.date.today()).strftime("%d/%m/%Y")
		#Time = (datetime.datetime.today()).strftime("%I %p")
		Time = (datetime.datetime.today()).strftime("%I:%M %p")
		DateTime= Date+" (" + Time + ")"
		Attendance[DateTime] = ["P" if Student in PresentStudents else "A" for Student  in StudentNames]
		# print("AddAttendance")
		
	path="E:\\Sem7\\HCI\\ProjAttendanceSystem\\HCI\\AttendanceSheets\\"
	#Writing to CSV File
	def WritetoCSVFile(courseName):
		#path="E:\\Sem7\\HCI\\ProjAttendanceSystem\\HCI\\AttendanceSheets\\"
		CourseName= path+courseName+".csv"
		Attendance.to_csv(CourseName,index=False)
		# print("Write TO CSV")
		
	#Reading From CSV File
	def ReadFromCSVFile(filename):
		filename = path+filename+".csv"
		Attendance = pd.read_csv(filename)
		return Attendance
		#print(Attendance)

	#Creating DataFrame With Student Names If Creating File For First Time
	def MakeNewDataFrame(StudentNames):
		Attendance = pd.DataFrame(StudentNames, columns = ["Student_Names"], index= [i+1 for i in range(len(StudentNames))])
		return Attendance
	
	if(os.path.isfile(path+courseName+".csv")):
		Attendance = ReadFromCSVFile(courseName)
	else:
		Attendance = MakeNewDataFrame(StudentNames)
	AddAttendance(Attendance,StudentNames,presentStudents)
	WritetoCSVFile(courseName)



	#*********************************************************************
	


	#**************************************** Edits *****************************************************************
except Exception as e:
	# print(e)
	print(-1)