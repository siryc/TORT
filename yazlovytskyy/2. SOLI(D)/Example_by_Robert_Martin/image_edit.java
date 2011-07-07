public void scaleToOneDimension(float desiredDimension, float imageDimension)
{ 
	if (Math.abs(desiredDimension - imageDimension) < errorThreshold) 
		return; 
	
	Image newlmage = ImageUtilities.getScaledImage(image, scalingFactor); 
	replaceImage(newImage);
} 
public void rotate(int degrees)
{ 
	Image newlmage = ImageUtilities.getRotatedImage(image, degrees); 
	replacelmage(newImage);
} 

private void replacelmage (Image newlmage) { 
	image.disposeO; 
	System.gc; 
	image = newlmage;
}

private float GetScalingFactor(float desiredDimension, float imageDimension)
{
	float scalingFactor = desiredDimension / imageDimension; 
	scalingFactor = (float)(Math.floor(scalingFactor * 100) * 0.01f): 
	return scalingFactor;
}  