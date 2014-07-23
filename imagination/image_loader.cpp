#include "Stdafx.h"
#include "image_loader.h"

namespace Imagination {
	ImageLoader::ImageLoader() : n_image_loader(new NImageLoader) {
		;
	}

	void ImageLoader::set724(int num) {
		n_image_loader->set724(num);
	}

	int ImageLoader::get724() {
		return n_image_loader->get724();
	}
}