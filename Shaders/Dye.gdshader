shader_type canvas_item;

void vertex() {
	// Called for every vertex the material is visible on.
}
uniform float ar;
uniform float ag;
uniform float ab;
uniform float add;


void fragment() {
	vec4 pixelColor = texture(TEXTURE, UV);
	//float add = 3.0/31.0;
	//float ar = 30.0 /31.0;
	//float ag = 25.0/31.0;
	//float ab = 1.0/31.0;
	float amax = 1.0;
	if (pixelColor.r == 0.0 && pixelColor.b == 0.0) {
		float n = pixelColor.g ;
		float r = n * ar / amax + add;
		float g = n * ag / amax  + add;
		float b = n * ab / amax + add;
		if (r > amax)
			r = amax;
		if (g > amax)
			g = amax;
		if (b > amax)
			b = amax;
		pixelColor = vec4(r / amax  , g / amax , b / amax  , pixelColor.a);
		COLOR = pixelColor;
	}
}

void light() {
	// Called for every pixel for every light affecting the CanvasItem.
}
