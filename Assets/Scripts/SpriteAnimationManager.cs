using UnityEngine;
using System.Collections;

public class SpriteAnimationManager : MonoBehaviour
{

    //vars for the whole sheet
    public int m_colCount = 4;
    public int m_rowCount = 4;

    //vars for animation
    private int m_rowNumber = 0; //Zero Indexed
    private int m_colNumber = 0; //Zero Indexed
    private int m_totalCells = 4;
    private int m_fps = 10;
    private bool m_clamping = false;
    private bool m_animationFinished;
    private float m_startTime;

    //Maybe this should be a private var
    private Vector2 m_offset;


    //Update
    void Update()
    {
        if (m_animationFinished) {
            return;
        }

        UpdateSpriteAnimation(m_colCount, m_rowCount, m_rowNumber, m_colNumber, m_totalCells, m_fps);
    }

    public void SetSpriteAnimation(SpriteAnimation animation)
    {
        m_rowNumber = animation.row;
        m_colNumber = animation.column;
        m_totalCells = animation.numFrames;
        m_fps = animation.fps;
        m_clamping = animation.clamping;
        m_startTime = Time.time;

        float sizeX = 1.0f / m_colCount;
        float sizeY = 1.0f / m_rowCount;
        Vector2 size = new Vector2(sizeX, sizeY);

        renderer.material.SetTextureScale("_MainTex", size);
    }

    private void UpdateSpriteAnimation(int colCount, int rowCount, int rowNumber, int colNumber, int totalCells, int fps)
    {
 
        // Calculate index
        int index = (int)(Time.time * fps);
        bool clampStop = (((Time.time - m_startTime) > 0.10f) && index % totalCells == 0);
        if (m_clamping && clampStop) {
            m_animationFinished = true;
            return;
        }

        // Repeat when exhausting all cells unless clamping
        index = index % totalCells;

        // Size of every cell
        float sizeX = 1.0f / colCount;
        float sizeY = 1.0f / rowCount;
        Vector2 size = new Vector2(sizeX, sizeY);

        // split into horizontal and vertical index
        var uIndex = index % colCount;
        var vIndex = index / colCount;

        // build offset
        // v coordinate is the bottom of the image in opengl so we need to invert.
        float offsetX = (uIndex + colNumber) * size.x;
        float offsetY = (1.0f - size.y) - (vIndex + rowNumber) * size.y;
        Vector2 offset = new Vector2(offsetX, offsetY);

        renderer.material.SetTextureOffset("_MainTex", offset);
        renderer.material.SetTextureScale("_MainTex", size);
    }
}
