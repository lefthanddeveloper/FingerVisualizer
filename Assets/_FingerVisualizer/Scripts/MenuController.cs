using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace FingerVisualizer 
{
    public class MenuController : MonoBehaviour
    {
        [SerializeField] private float expandTime = 0.4f;
        [SerializeField] private MenuButton menuButton;
        [SerializeField] private MenuDropDown menuDropdown;
        [SerializeField] private Menu[] menus;

        void Start()
        {
            Init();
        }

        private void Init()
		{
            menuButton.Init(OnClickMenuButton);
            menuDropdown.Init();
            foreach (var menu in menus) menu.Init();
		}

        private void OnClickMenuButton()
		{
            menuButton.Spin(expandTime);

			if (menuDropdown.isExpanded)
			{
                menuDropdown.Condense(expandTime * 1.5f);
                for(int i=0; i< menus.Length; i++)
				{
                    menus[i].OnCondense(expandTime * 1.5f);
                    menus[i].HidePanel();
                }
			}
			else
			{
                menuDropdown.Expand(expandTime * 1.5f);
                for (int i = 0; i < menus.Length; i++)
                {
                    menus[i].OnExpand(expandTime * 1.5f);
                    
                }
            }

		}

        public void OnMenuSelected(Menu selectedMenu)
		{
            for(int i=0; i< menus.Length; i++)
			{
                if(menus[i] == selectedMenu)
				{
                    menus[i].ShowPanel();
				}
				else
				{
                    menus[i].HidePanel();
				}
			}
		}

    }

}

