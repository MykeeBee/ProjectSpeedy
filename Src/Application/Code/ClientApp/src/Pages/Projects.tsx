import { useState, useEffect, Dispatch, SetStateAction } from 'react';
import { CardGrid, CardItem } from '../Components/CardGrid'
import { IPage } from '../Interfaces/IPage';
import { ProjectService } from '../Services/ProjectService'
import ProjectNewForm from '../Components/Projects/ProjectNewForm'

export function Projects(pageProps: IPage)
{
  /**
   * Existing projects.
   */
  const [projects, setProjects]: [CardItem[], Dispatch<SetStateAction<CardItem[]>>] = useState(new Array<CardItem>());

  /**
   * Used to run code only once on page load.
   */
  const [runOnce, setRunOnce] = useState(false);
  useEffect(() =>
  {
    if (runOnce === false)
    {
      document.title = 'Projects';
      pageProps.setBreadCrumbs([{ text: "Projects", address: "/", isLast: true }]);
      setRunOnce(true);

      // Loads the projects onto the page
      ProjectService.GetAll().then(
        (data) =>
        {
          // Update the grid.
          setProjects(data);
        },
        (error) =>
        {
          alert(error);
        }
      );
    }
  }, [runOnce, pageProps]);

  return (<>
    <div className="row">
      <div className="col">
        <h1>Projects</h1>
      </div>
    </div>
    <div>
      <h1>Anywhere in your app!</h1>
    </div>
    <CardGrid data={projects} />
    <ProjectNewForm setProjects={(projects: CardItem[]) =>setProjects(projects)}/>
  </>);
}