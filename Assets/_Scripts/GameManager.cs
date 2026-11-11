using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public Transform player;
    public CameraController cameraController;
    public Transform[] goals;

    public Button btnKick;
    public Button btnAutoKick;
    public Button btnReset;

    public float kickDistance = 2.5f;

    private List<Ball> balls = new List<Ball>();
    private Ball nearestBall;

    void Start()
    {   
        balls.AddRange(FindObjectsOfType<Ball>());

        btnKick.onClick.AddListener(OnKickClicked);
        btnAutoKick.onClick.AddListener(OnAutoKickClicked);
        btnReset.onClick.AddListener(OnResetClicked);
    }

    void Update()
    {
        CheckDistance();
    }

    void CheckDistance()
    {
        nearestBall = GetNearestBall();

        if(nearestBall != null)
        {
            float dist = Vector3.Distance(getPlayerFlatPosition(player), getBallFlatPosition(nearestBall));

            if (dist <= kickDistance && !nearestBall.IsFlying())
            {
                btnKick.gameObject.SetActive(true);
            }
            else
            {
                btnKick.gameObject.SetActive(false);
            }
        }
        else
        {
            btnKick.gameObject.SetActive(false);
        }
    }

    Vector3 getPlayerFlatPosition(Transform player)
    {
        return new Vector3(player.position.x, 0f, player.position.z);
    }

    Vector3 getBallFlatPosition(Ball ball)
    {
        return new Vector3(ball.transform.position.x, 0f, ball.transform.position.z);
    }

    Ball GetNearestBall()
    {
        Ball nearest = null;
        float minDist = Mathf.Infinity;

        foreach (var ball in balls)
        {
            if (ball == null || ball.IsFlying()) continue;

            float dist = Vector3.Distance(getPlayerFlatPosition(player), getBallFlatPosition(ball));
            if (dist < minDist)
            {
                minDist = dist;
                nearest = ball;
            }
        }
        return nearest;
    }

    Ball GetFurthestBall()
    {
        Ball furthest = null;
        float maxDist = -1f;

        foreach (var ball in balls)
        {
            if (ball == null || ball.IsFlying()) continue;

            float dist = Vector3.Distance(getPlayerFlatPosition(player), getBallFlatPosition(ball));

            if (dist > maxDist)
            {
                maxDist = dist;
                furthest = ball;
            }
        }
        return furthest;
    }

    Transform GetNearestGoal(Vector3 ballPosition)
    {
        Transform nearestGoal = goals[0];
        float minDist = Mathf.Infinity;

        foreach (var goal in goals)
        {
            float dist = Vector3.Distance(ballPosition, goal.position);
            if(dist < minDist)
            {
                minDist = dist;
                nearestGoal = goal;
            }
        }
        return nearestGoal;
    }

    void ExecuteKick(Ball ball)
    {
        balls.Remove(ball);

        Transform nearestGoal = GetNearestGoal(ball.transform.position);
        cameraController.ChangeTarget(ball.transform);
        ball.ShootTo(nearestGoal.position, () =>
        {
            cameraController.ReturnToPlayer(2f);
        });
    }

    void OnKickClicked()
    {
        if (nearestBall == null) return;
        ExecuteKick(nearestBall);
    }

    void OnAutoKickClicked()
    {
        Ball furthestBall = GetFurthestBall();
        if (furthestBall == null) return;
        ExecuteKick(furthestBall);
    }

    void OnResetClicked()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

}
