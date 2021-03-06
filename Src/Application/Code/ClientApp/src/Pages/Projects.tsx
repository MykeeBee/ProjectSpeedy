import { useState, useEffect, Dispatch, SetStateAction } from 'react';
import { CardGrid, CardItem } from '../Components/CardGrid'
import { IPage } from '../Interfaces/IPage';
import { ProjectService } from '../Services/ProjectService'
import ProjectNewForm from '../Components/Projects/ProjectNewForm'
import { PageFunctions } from './PageFunctions';
import { BreadCrumbItem } from '../Components/BreadCrumbs';

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
      
      // Hides any previous messages
      pageProps.globalMessageHide();
      pageProps.setBreadCrumbs(new Array<BreadCrumbItem>())

      // We only want to run this once on page load.
      setRunOnce(true);

      // Loads the projects onto the page
      ProjectService.GetAll().then(
        (data) =>
        {
          setProjects(data);
        },
        (error) =>
        {
          alert(error);
        }
      );
    }
  }, [runOnce, pageProps]);

  /**
   * Whether the dialog has even been opened.
   */
  const [dialogOpened, setDialogOpened] = useState(false);

  return (<>
    <div className="row">
      <div className="col">
        <h1>Projects</h1>
        <p>A project allows you to group problems and bets together.</p>
      </div>
    </div>
    <CardGrid data={projects}  AddNewClick={(e) => { PageFunctions.DisplayModal(e, dialogOpened, (newValue) => { setDialogOpened(newValue) }) }} />
    <ProjectNewForm setProjects={(data: CardItem[]) => setProjects(data)} pageProps={pageProps}/>
  </>);
}