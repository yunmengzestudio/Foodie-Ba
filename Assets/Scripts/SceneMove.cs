﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneMove : MonoBehaviour {

    public string DefaultNextScene;
    public bool FastMode = false;

    [Header("Configure")]
    public string AutoLoadScene = "";
    public float AutoLoadTime = 5f;
    public bool DebugLog = true;
    public int StepOfProgress = 2;     // 平滑进度的间隔
    public Text Text;
    public Slider Slider;

    [Header("Scene Transition")]
    public Animator Animator;
    

    private IEnumerator Start() {
        if (Text)
            Text.gameObject.SetActive(false);
        if (Slider)
            Slider.gameObject.SetActive(false);
        if (AutoLoadScene != "") {
            yield return new WaitForSeconds(AutoLoadTime);
            Load(AutoLoadScene);
        }
        GameManager.Instance.SceneMove = this;
    }

    public void Load(string sceneName = "") {
        // 加载场景 需要重置 GameManager
        GameManager.Instance.ResetConf();

        if (sceneName == "") {
            sceneName = DefaultNextScene;
        }
        if (FastMode)
            StartCoroutine(AsyncLoadScene_Fast(sceneName));
        else
            StartCoroutine(AsyncLoadScene(sceneName));
    }

    // 异步加载场景，进度条 Value 有过渡
    private IEnumerator AsyncLoadScene(string sceneName) {
        // 场景切换动画
        if (Animator) {
            Animator.SetTrigger("End");
            while (!Animator.GetCurrentAnimatorStateInfo(0).IsName("End")) {
                yield return new WaitForEndOfFrame();
            }
            float sec = Animator.GetCurrentAnimatorStateInfo(0).length;
            yield return new WaitForSeconds(sec);
        }
        // 显示进度条
        ActivateProcessBar();

        int progress = 0, targetProgress = 0;
        AsyncOperation op = SceneManager.LoadSceneAsync(sceneName);  // Application.LoadLevelAsync()
        op.allowSceneActivation = false;

        while (op.progress < 0.9f) {
            targetProgress = (int)(op.progress * 100);
            while (progress < targetProgress) {
                progress += StepOfProgress;
                SetProcessBar(progress);
                yield return new WaitForEndOfFrame();
            }
        }

        targetProgress = 100;
        while (progress < targetProgress) {
            progress += StepOfProgress;
            progress = progress > 100 ? 100 : progress;
            SetProcessBar(progress);
            yield return new WaitForEndOfFrame();
        }
        
        yield return new WaitForEndOfFrame();
        op.allowSceneActivation = true;
    }

    // 快速加载（原速），进度条 Value 无过渡
    private IEnumerator AsyncLoadScene_Fast(string sceneName) {
        // 场景切换动画
        if (Animator) {
            Animator.SetTrigger("End");
            while (!Animator.GetCurrentAnimatorStateInfo(0).IsName("End")) {
                yield return new WaitForEndOfFrame();
            }
            float sec = Animator.GetCurrentAnimatorStateInfo(0).length;
            yield return new WaitForSeconds(sec);
        }
        // 显示进度条
        ActivateProcessBar();

        AsyncOperation op = SceneManager.LoadSceneAsync(sceneName);
        op.allowSceneActivation = false;
        while (op.progress < 0.9f) {
            SetProcessBar(op.progress * 100);
            yield return new WaitForEndOfFrame();
        }
        SetProcessBar(100);
        yield return new WaitForEndOfFrame();
        op.allowSceneActivation = true;
    }

    private void ActivateProcessBar(bool active = true) {
        if (Text)
            Text.gameObject.SetActive(active);
        if (Slider)
            Slider.gameObject.SetActive(active);
    }

    private void SetProcessBar(float process) {
        if (DebugLog && !Text && !Slider) {
            Debug.Log("Invisible Process Bar: " + process + "%");
            return;
        }
        if (Text) {
            Text.text = process.ToString() + "%";
        }
        if (Slider) {
            Slider.value = process / 100;
        }
    }
}
