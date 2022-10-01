using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rope : MonoBehaviour
{
  private LineRenderer lineRenderer;
  private List<RopeSegment> ropeSegments = new List<RopeSegment>();
  private float ropeSegLen = 0.25f;
  private int segmentLength = 35;
  private float lineWidth = 0.1f;

  void Start()
  {
    this.lineRenderer = this.GetComponent<LineRenderer>();
    Vector3 ropeStartPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);

    for (int i = 0; i < segmentLength; i++)
    {
      this.ropeSegments.Add(new RopeSegment(ropeStartPoint));
      ropeStartPoint.y -= ropeSegLen;
    }
  }

  void Update()
  {
    this.DrawRope();
  }

  private void DrawRope()
  {
    float lineWidth = this.lineWidth;
    lineRenderer.startWidth = lineWidth;
    lineRenderer.endWidth = lineWidth;

    Vector3[] ropePositions = new Vector3[this.segmentLength];
    for (int i = 0; i < this.segmentLength; i++)
    {
      ropePositions[i] = this.ropeSegments[i].posNow;
    }

    lineRenderer.positionCount = ropePositions.Length;
    lineRenderer.SetPositions(ropePositions);
  }

  public struct RopeSegment
  {
    public Vector2 posNow;
    public Vector2 posOld;

    public RopeSegment(Vector2 pos)
    {
      this.posNow = pos;
      this.posOld = pos;
    }
  }
}
