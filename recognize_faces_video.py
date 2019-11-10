
# import the necessary packages
from imutils.video import VideoStream
import face_recognition
import argparse
import imutils
import pickle
import time
import cv2


#************************* Edits ***************************


upper_left = (200, 200)
bottom_right = (400, 400)
presentStudents = list()
camSource = 0 # 0 for laptop cam and 1 for external cam
encodings = 'encodings2.pickle'

#************************************************************

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

		if(left > a and left < c and top > b and top < d and right < c and bottom < d):
			

			if(not presentStudents.__contains__(name)and ( not name.__contains__('Unknown')) ):
				presentStudents.append(name)
			if not name.__contains__('Unknown'):
				cv2.putText(frame, name+"Present", (30, 20),cv2.FONT_HERSHEY_SIMPLEX, 1, (195, 100, 255), 1)

			#*********************************************************************************************************

	cv2.imshow("Frame", frame)
	key = cv2.waitKey(1) & 0xFF

	# if the `q` key was pressed, break from the loop
	if key == ord("q"):
		break

# do a bit of cleanup
cv2.destroyAllWindows()
vs.stop()

#**************************************** Edits *****************************************************************

import os
print('Total Students:	',(len(next(os.walk('dataset'))[1]))-1)
# print('Total Students:	',4)

print("Total Students Present:",presentStudents.__len__())

for i in presentStudents:
	print(i)




#**************************************** Edits *****************************************************************
