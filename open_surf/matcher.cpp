
#include "surflib.h"
#include "kmeans.h"
#include <ctime>
#include <iostream>
#include "opencv2\imgproc\imgproc.hpp"
#include "opencv2\highgui\highgui.hpp"
#include "matcher.h"
#include <cstdio>
#include <vector>

using namespace std;

Matcher::Matcher(char *base, char** images, int size) {
	char *hello;
	char buf[1024];

	int x = 3, itr = 0;

	hello = images[0];
	IplImage *img1, *img2, *img11, *img22;


	for (itr = 0; itr < size; itr++) {
		img1 = cvLoadImage(base);
		img2 = cvLoadImage(images[itr]);

		

		IpVec ipts1, ipts2;
		surfDetDes(img1, ipts1, false, 4, 4, 2, 0.0001f);
		surfDetDes(img2, ipts2, false, 4, 4, 2, 0.0001f);

		vector<cv::Point2f> src_points;
		vector<cv::Point2f> tgt_points;
		IpPairVec matches;
		getMatches(ipts1, ipts2, matches);
		cv::Point2f *temp;
		for (int i = 0; i<matches.size() - 1; i++){
			temp = new cv::Point2f(matches[i].first.x, matches[i].first.y);
			tgt_points.push_back(*temp);
			temp = new cv::Point2f(matches[i].second.x, matches[i].second.y);
			src_points.push_back(*temp);
		}
		cv::Mat warp_mat = cv::findHomography(src_points, tgt_points, CV_LMEDS);

		cv::Mat mat1 = cv::cvarrToMat(img1);
		cv::Mat mat2 = cv::cvarrToMat(img2);
		cv::Mat mat3;
		

		
		cv::warpPerspective(mat2, mat2, warp_mat, mat2.size());
		
		IplImage *im_gray = cvCreateImage(cvGetSize(img1), IPL_DEPTH_8U, 1);
		cvCvtColor(img1, im_gray, CV_RGB2GRAY);
		cvThreshold(im_gray, im_gray, 32, 255, CV_THRESH_BINARY | CV_THRESH_OTSU);
		img1 = im_gray;
		IplImage *im_gray2 = cvCreateImage(cvGetSize(img2), IPL_DEPTH_8U, 1);
		cvCvtColor(img2, im_gray2, CV_RGB2GRAY);
		cvThreshold(im_gray2, im_gray2, 32, 255, CV_THRESH_BINARY | CV_THRESH_OTSU);
		img2 = im_gray2;


		std::cout << "Matches: " << matches.size();

		mat1 = cv::cvarrToMat(img1, true);
		mat2 = cv::cvarrToMat(img2, true);
		mat3 = cv::cvarrToMat(img1);

		for (int i = 1; i < mat1.rows - 1; ++i)
		{
			for (int j = 1; j < mat1.cols - 1; ++j)
			{
				if (mat2.ptr<uchar>(i)[j] < 128) {
					mat3.ptr<uchar>(i)[j] = 0;

					for (int dx = -1; dx <= 1; dx++) {
						for (int dy = -1; dy <= 1; dy++) {
							if (dx + i >= 0 && dy + j >= 0 &&
								dx + i < mat1.rows &&
								dy + j < mat1.cols &&
								mat1.ptr<uchar>(i + dx)[j + dy] < 128) {
								mat3.ptr<uchar>(i)[j] = 255;
							}
						}
					}
				}
				else {
					mat3.ptr<uchar>(i)[j] = 255;

				}

			}
		}

		sprintf(buf, "%s_out.jpg", images[itr]);
		cvSaveImage(buf, img1);
	}
}