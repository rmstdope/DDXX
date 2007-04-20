#ifndef EFFECTS_CONSTANTS_H
#define EFFECTS_CONSTANTS_H

/**
 * Light Types
 */
#define DIRECTIONAL 0
#define POINT 1
#define SPOT 2

/**
 * Definitions of blend operations
 */
#define ADD 1
#define SUBTRACT 2
#define REVSUBTRACT 3
#define MIN 4
#define MAX 5

/**
 * Definitions of blend modes
 */
#define ZERO 1
#define ONE 2
#define SRCCOLOR 3
#define INVSRCCOLOR 4
#define SRCALPHA 5
#define INVSRCALPHA 6
#define DESTALPHA 7
#define INVDESTALPHA 8
#define DESTCOLOR 9
#define INVDESTCOLOR 10
#define SRCALPHASAT 11
#define BOTHSRCALPHA 12
#define BOTHINVSRCALPHA 13
#define BLENDFACTOR 14
#define INVBLENDFACTOR 15

/**
 * Definitions of fill modes
 */
#define FILLMODE_POINT 1
#define FILLMODE_WIREFRAME 2
#define FILLMODE_SOLID 3

/**
 * Other constants
 */
#define MAX_NUM_BONES 40
#define PIx2 6.283185307179586476925286766559

#endif
