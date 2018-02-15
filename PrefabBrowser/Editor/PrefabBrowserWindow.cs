using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace HypnagogiaGames.PrefabBrowser
{
    namespace Assets.Code.Editor
    {
        public class PrefabBrowserWindow : EditorWindow
        {
            // SerializeField is used to ensure the view state is written to the window 
            // layout file. This means that the state survives restarting Unity as long as the window
            // is not closed. If the attribute is omitted then the state is still serialized/deserialized.
            [SerializeField] TreeViewState m_TreeViewState;

            //The TreeView is not serializable, so it should be reconstructed from the tree data.
            PrefabBrowserTreeView m_SimpleTreeView;

            GameObject targetObject;
            public GameObject TargetObject
            {
                get { return targetObject; }
                set { targetObject = value; }
            }

            [MenuItem("Window/Hypnagogia Games/Prefab Browser")]
            static void ShowWindow()
            {
                var window = GetWindow<PrefabBrowserWindow>();
                window.titleContent = new GUIContent("Prefab Browser");
                window.Show();
            }

            void OnEnable()
            {
                // Check whether there is already a serialized view state (state 
                // that survived assembly reloading)
                if (m_TreeViewState == null)
                    m_TreeViewState = new TreeViewState();

                m_SimpleTreeView = new PrefabBrowserTreeView(m_TreeViewState);
            }

            void OnGUI()
            {
                TargetObject = (GameObject)EditorGUI.ObjectField(new Rect(3, 3, position.width - 6, 20), "Target Object", TargetObject, typeof(GameObject), false);
                m_SimpleTreeView.SetTarget(TargetObject);
                m_SimpleTreeView.OnGUI(new Rect(0, 20, position.width, position.height));
            }
        }
    }
}
