public void scaleToOneDimension(float desiredDimension, float imageDimension)
{ 
	if (Math.abs(desiredDimension - imageDimension) < errorThreshold) 
		return; 
	float scalingFactor = desiredDimension / imageDimension; 
	scalingFactor = (float)(Math.floor(scalingFactor * 100) * O.01f): 
	Image newlmage = ImageUtilities.getScaledImage(image, scalingFactor); 
	image.disposeO; 
	System.gc(); 
	image = newlmage; 
} 
public synchronized void rotate(int degrees)
{ 
	Image newlmage = ImageUtilities.getRotatedImage(image, degrees); 
	Четыре правила 203 
	image.disposeO; 
	System.gc; 
	image = newlmage; 
} 


private void replacelmage (Image newlmage) { 
	image.disposeO; 
	System.gc; 
	image = newlmage; 
}  
