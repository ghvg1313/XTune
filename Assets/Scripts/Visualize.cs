using UnityEngine;
using System.Collections;

public class Visualize : MonoBehaviour {


	public GameObject[] levelBars;
	public float radius = 10f;

	public bool start = false;

	private float toRadius = Mathf.PI / 180f;
	public GameObject m_LevelLeft;
	public GameObject m_LevelRight;
	private bool m_IsOk = false;
	private int m_NumSamples = 1024;
	private float[] m_SamplesL, m_SamplesR;
	private float[] levels;

	private int i;
	private float maxL, maxR, sample, sumL, sumR, rms, dB;
	private Vector3 scaleL, scaleR;
	// Because rms values are usually very low
	private float volume = 30.0f;
	private Color color;

	public float colorBlue = 0f;

	// Use this for initialization
	void Start () {
		levelBars = GameObject.FindGameObjectsWithTag("Bar");
		//radius = 10f;
		m_NumSamples = levelBars.Length;
		levels = new float[m_NumSamples];
		//levels = new float[(int)m_NumSamples/levelBars.Length+1];

		// Just proper validation
		/*
		if (m_LevelLeft != null && m_LevelRight != null) {
			m_SamplesL = new float[m_NumSamples];
			m_SamplesR = new float[m_NumSamples];

			scaleL = m_LevelLeft.transform.localScale;
			scaleR = m_LevelRight.transform.localScale;
			m_IsOk = true;
			audio.Play();
		}*/

		if (levelBars.Length != 0) {

			m_SamplesL = new float[m_NumSamples];
			m_SamplesR = new float[m_NumSamples];

			for(int i=0;i<levelBars.Length;i++){
				scaleL = levelBars[i].transform.localScale;
				Vector3 position = levelBars[i].transform.position;
				position.z = radius * Mathf.Sin(toRadius*360f/levelBars.Length*i);
				position.x = radius * Mathf.Cos(toRadius*360f/levelBars.Length*i);
				levelBars[i].transform.position = position;
			}
			m_IsOk = true;
			//audio.Play();
		}

		else
			Debug.Log("Missing objects linkage");
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey (KeyCode.U))
			colorBlue += 0.01f;

		if (start && !audio.isPlaying) {
			audio.Play();
		}
		/*
		// Continuing proper validation
		if (m_IsOk) {
			audio.GetOutputData(m_SamplesL, 0);
			audio.GetOutputData(m_SamplesR, 1);
			maxL = maxR = 0.0f;
			sumL = 0.0f;
			sumR = 0.0f;
			for (i = 0; i < m_NumSamples; i++) {
				sumL += m_SamplesL[i] * m_SamplesL[i];
				sumR += m_SamplesR[i] * m_SamplesR[i];
			}
			rms = Mathf.Sqrt(sumL/m_NumSamples);
			scaleL.y = rms;
			//scaleL.y = Mathf.Clamp01(rms*volume);
			rms = Mathf.Sqrt(sumR/m_NumSamples);
			scaleR.y = rms;
			//scaleR.y = Mathf.Clamp01(rms*volume);

			scaleL.y *=8f;
			scaleR.y *=8f;

			m_LevelLeft.transform.localScale = scaleL;
			m_LevelRight.transform.localScale = scaleR;
			color = GetVolumeColor(scaleL.y);
			m_LevelLeft.GetComponentInChildren<Renderer>().material.color = color;
			color = GetVolumeColor(scaleR.y);
			m_LevelRight.GetComponentInChildren<Renderer>().material.color = color;
		}*/
		if (m_IsOk) {
			audio.GetOutputData(m_SamplesL, 0);
			audio.GetOutputData(m_SamplesR, 1);
			maxL = maxR = 0.0f;
			sumL = 0.0f;
			sumR = 0.0f;

			int levelCurrent = 0;

			//for(i=0;i<levels.Length;i++)
				//levels[i] = 0f;

			for (i = 0; i < m_NumSamples; i++) 
			{
				levels[i] = m_SamplesL[i]*m_SamplesR[i];
			}

			for(i=0;i<m_NumSamples;i++){
				//rms = Mathf.Sqrt(levels[i]);
				scaleL.y = levels[i];
				scaleL.y *= 16;
				scaleL.y *= scaleL.y;

				levelBars[i].transform.localScale = Vector3.Lerp(levelBars[i].transform.localScale, scaleL, 10*Time.deltaTime);
				Vector3 position = levelBars[i].transform.position;
				position.y = levelBars[i].transform.localScale.y /2-1.2f;
				position.z = radius * Mathf.Sin(toRadius*360f/levelBars.Length*i);
				position.x = radius * Mathf.Cos(toRadius*360f/levelBars.Length*i);
				levelBars[i].transform.position = position;

				color = GetVolumeColor(scaleL.y);
				levelBars[i].GetComponentInChildren<Renderer>().material.color = color;
			}
		}
	}
	
	Color GetVolumeColor (float volume) {
		volume /= 8f;
		/*
		if (volume > 0.7f)
			return Color.red;
		if (volume > 0.5f)
			return Color.yellow;*/
		return new Color (volume, 1 - volume, colorBlue);
		//return Color.green;
	}
	
	
}
