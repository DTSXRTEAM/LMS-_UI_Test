using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Video;

[System.Serializable]
public class CourseChapter
{
    public string CourseChapterTitle;
    public List<CourseSection> CourseSection;
    public List<GameObject> CourseSectionCards;
}

public enum SectionContentType
{
    Video,
    Text
}

[System.Serializable]
public class CourseSection
{
    public string CourseSectionTitle;
    public SectionContentType contentType;
    public GameObject textContent;
    public GameObject videoContainer;  
}

public class CourseContentManager : MonoBehaviour
{
    public Transform Container;
    public GameObject ChapterCardPrefab;
    public GameObject SectionCardPrefab;
    int currentChapter = -1;
    int currentSection = -1;
    public VideoPlayer videoPlayer;
    public GameObject videoPanel;
    GameObject activeContent;
    public GameObject textPanel;
    public List<CourseChapter> m_courseChapters = new List<CourseChapter>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for(int i =0; i<m_courseChapters.Count; i++)
        {
            int chapterIndex = i;

            GameObject ChapterCard = Instantiate(ChapterCardPrefab, Container);
            ChapterCard.GetComponent<ChapterCardReference>().ChapterTitle.text = $"Chapter {i+1} : {m_courseChapters[i].CourseChapterTitle}";
            //button == arg entry on click toggleCard(i)

            ChapterCard.GetComponent<ChapterCardReference>().ClickButton.onClick.AddListener(() => toggleCard(chapterIndex));
        
            for(int j=0; j < m_courseChapters[i].CourseSection.Count; j++)
            {
                int sectionIndex = j;

                GameObject Sectioncard = Instantiate(SectionCardPrefab, Container);
                Sectioncard.GetComponent<SectionCardReference>().SectionTitle.text = $"Section {j+1} : {m_courseChapters[i].CourseSection[j].CourseSectionTitle}";
                m_courseChapters[i].CourseSectionCards.Add(Sectioncard);

                Sectioncard.GetComponent<SectionCardReference>()
                .ClickButton
                .onClick
                .AddListener(() => OnSectionClicked(chapterIndex, sectionIndex));
            }
        }

            // Auto-select first section on start (optional but recommended)
        if (m_courseChapters.Count > 0 &&
            m_courseChapters[0].CourseSection.Count > 0)
        {
            OnSectionClicked(0, 0);
        }  
    }

    public void toggleCard(int chatercard)
    {
        foreach (GameObject item in m_courseChapters[chatercard].CourseSectionCards)
        {
            if (item.activeSelf)
                item.SetActive(false);
            else
                item.SetActive(true);
        }
    }

  void OnSectionClicked(int chapterIndex, int sectionIndex)
{
    currentChapter = chapterIndex;
    currentSection = sectionIndex;

    // Hide previous content
    if (activeContent != null)
    {
        // Stop video if previous was a video container
        VideoPlayer vp = activeContent.GetComponentInChildren<VideoPlayer>();
        if (vp != null && vp.isPlaying)
            vp.Stop();

        activeContent.SetActive(false);
    }

    CourseSection section =
        m_courseChapters[chapterIndex].CourseSection[sectionIndex];

    if (section.contentType == SectionContentType.Text)
    {
        activeContent = section.textContent;
    }
    else // Video
    {
        activeContent = section.videoContainer;
    }

    if (activeContent != null)
        activeContent.SetActive(true);
}

public void NextSection()
{
    if (currentChapter == -1 || currentSection == -1)
        return;

    int nextSection = currentSection + 1;

    // Stop if last section
    if (nextSection >= m_courseChapters[currentChapter].CourseSection.Count)
        return;

    OnSectionClicked(currentChapter, nextSection);
}

public void PreviousSection()
{
    if (currentChapter == -1 || currentSection == -1)
        return;

    int prevSection = currentSection - 1;

    // Stop if first section
    if (prevSection < 0)
        return;

    OnSectionClicked(currentChapter, prevSection);
}

    /*void ShowVideoPage(CourseSection section)
    {
        textPanel.SetActive(false);
        videoPanel.SetActive(true);

        // Hook VideoPlayer here (DO NOT autoplay)
        Debug.Log("Open VIDEO section: " + section.CourseSectionTitle);
    }

    void ShowTextPage(CourseSection section)
    {
        videoPanel.SetActive(false);
        textPanel.SetActive(true);

        Debug.Log("Open TEXT section: " + section.CourseSectionTitle);
    }*/
}
