MGFX vs_uniforms_vec4@     S  #ifdef GL_ES
precision mediump float;
precision mediump int;
#endif

const vec4 ps_c0 = vec4(0.0, 0.0, 0.0, 0.0);
vec4 ps_r0;
varying vec4 vTexCoord0;
#define ps_t0 vTexCoord0
#define ps_oC0 gl_FragColor

void main()
{
	ps_r0.x = ps_t0.y;
	ps_r0.y = ps_t0.x;
	ps_r0.z = ps_t0.z;
	ps_r0.w = ps_c0.x;
	ps_oC0 = ps_r0;
}

     #ifdef GL_ES
precision highp float;
precision mediump int;
#endif

uniform vec4 vs_uniforms_vec4[4];
uniform vec4 posFixup;
const vec4 vs_c4 = vec4(1.0, 0.0, 0.0, 0.0);
#define vs_c0 vs_uniforms_vec4[0]
#define vs_c1 vs_uniforms_vec4[1]
#define vs_c2 vs_uniforms_vec4[2]
#define vs_c3 vs_uniforms_vec4[3]
attribute vec4 vs_v0;
#define vs_oPos gl_Position
varying vec4 vFrontColor;
#define vs_oD0 vFrontColor
varying vec4 vTexCoord0;
#define vs_oT0 vTexCoord0

void main()
{
	vs_oPos.x = dot(vs_v0, vs_c0);
	vs_oPos.y = dot(vs_v0, vs_c1);
	vs_oPos.z = dot(vs_v0, vs_c2);
	vs_oPos.w = dot(vs_v0, vs_c3);
	vs_oD0 = vs_v0.yxzy * vs_c4.xxxy;
	vs_oT0 = vs_v0;
	gl_Position.y = gl_Position.y * posFixup.y;
	gl_Position.xy += posFixup.zw * gl_Position.ww;
}

  vs_v0    xViewProjection                                                                    Simplest Pass0     