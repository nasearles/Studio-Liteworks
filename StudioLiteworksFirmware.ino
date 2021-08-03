#include <FastLED.h>

#define NUM_LEDS 60

CRGB leds[NUM_LEDS];

int getCommand;
byte prev;
byte rgb[4];
int frames = 120;



void setup() {
  // put your setup code here, to run once:
  
  FastLED.addLeds<WS2812, 9, GRB>(leds, 60);
  for(int i = 0; i < 60;i++){
    leds[i] = CRGB(0,0,0);
  }
  FastLED.show();
  Serial.begin(9600);

}

typedef void (*SimplePatternList[])();
SimplePatternList gPatterns = { solid, rainbow, rainbowWithGlitter, confetti, sinelon, juggle, bpm };

uint8_t gCurrentPatternNumber = 0; // Index number of which pattern is current
uint8_t gHue = 0; // rotating "base color" used by many of the patterns

void loop() {
  // put your main code here, to run repeatedly:
  if(Serial.available() > 0){
    Serial.readBytes(rgb, 4);
    if(rgb[0] == 8){
      FastLED.setBrightness(rgb[1]);
      rgb[0] = prev;
    }
    else if(rgb[0] == 9){
      frames = (int)rgb[1];
      rgb[0] = prev;
    }
    else
      prev = rgb[0];
  }
  gPatterns[prev]();
  FastLED.delay(255/frames); 
  FastLED.show();

  EVERY_N_MILLISECONDS( 20 ) { gHue++; }
}

#define ARRAY_SIZE(A) (sizeof(A) / sizeof((A)[0]))

void solid()
{
      for(int i = 0; i < 60;i++){
        leds[i] = CRGB(rgb[1]/2,rgb[2]/2,rgb[3]/2);
      }
}

void rainbow() 
{
  // FastLED's built-in rainbow generator
  fill_rainbow( leds, NUM_LEDS, gHue, 7);
}

void rainbowWithGlitter() 
{
  // built-in FastLED rainbow, plus some random sparkly glitter
  rainbow();
  addGlitter(80);
}

void addGlitter( fract8 chanceOfGlitter) 
{
  if( random8() < chanceOfGlitter) {
    leds[ random16(NUM_LEDS) ] += CRGB::White;
  }
}

void confetti() 
{
  // random colored speckles that blink in and fade smoothly
  fadeToBlackBy( leds, NUM_LEDS, 10);
  int pos = random16(NUM_LEDS);
  leds[pos] += CHSV( gHue + random8(64), 200, 255);
}

void sinelon()
{
  // a colored dot sweeping back and forth, with fading trails
  fadeToBlackBy( leds, NUM_LEDS, 20);
  int pos = beatsin16( 13, 0, NUM_LEDS-1 );
  leds[pos] += CHSV( gHue, 255, 192);
}

void bpm()
{
  // colored stripes pulsing at a defined Beats-Per-Minute (BPM)
  uint8_t BeatsPerMinute = 62;
  CRGBPalette16 palette = PartyColors_p;
  uint8_t beat = beatsin8( BeatsPerMinute, 64, 255);
  for( int i = 0; i < NUM_LEDS; i++) { //9948
    leds[i] = ColorFromPalette(palette, gHue+(i*2), beat-gHue+(i*10));
  }
}

void juggle() {
  // eight colored dots, weaving in and out of sync with each other
  fadeToBlackBy( leds, NUM_LEDS, 20);
  byte dothue = 0;
  for( int i = 0; i < 8; i++) {
    leds[beatsin16( i+7, 0, NUM_LEDS-1 )] |= CHSV(dothue, 200, 255);
    dothue += 32;
  }
}
