import React, { useState, useEffect, Dispatch } from 'react';
import { useParams } from "react-router-dom";
import ProblemBetNewForm from '../Components/ProblemBetNewForm';
import { CardGrid, CardItem } from '../Components/CardGrid'
import { IPage, IProblem } from '../Interfaces/IPage';
import { ProblemService } from '../Services/ProblemService';
import { PageFunctions } from './PageFunctions';
import { BetService } from '../Services/BetService';
import ProblemUpdateForm from '../Components/Problem/ProblemUpdateForm';

export function Problem(pageProps: IPage)
{
    /**
     * GET parameters.
     */
    let { problemId, projectId }: { problemId: string, projectId: string } = useParams();

    /**
     * Page model definition.
     */
    var defaultProblem: IProblem = { name: "", successCriteria:"", description:"", bets: new Array<CardItem>(), isLoaded: false };
    const [problem, setProblem]: [IProblem, Dispatch<IProblem>] = useState(defaultProblem);

    /**
     * Used to run code only once on page load.
     */
    const [runOnce, setRunOnce] = useState(false);
    useEffect(() =>
    {
        if (runOnce === false)
        {
            // Hides any previous messages
            pageProps.globalMessageHide();

            // We only want to run this once on page load.
            setRunOnce(true);

            // Loads the projects onto the page
            ProblemService.Get(projectId, problemId).then(
                (data) =>
                {
                    // Sets the model against the page.
                    setProblem(data);
                    data.isLoaded = true;

                    // Sets the project name.
                    document.title = `Problem ${problem.name}`;

                    // Set the breadcrumbs.
                    pageProps.setBreadCrumbs([
                        { address: "/", text: "Projects", isLast: false },
                        { address: `/project/${projectId}`, text: "Project Name", isLast: false },
                        { address: `/project/${projectId}/problem/${problemId}`, text: "ProblemName", isLast: true }
                    ]);
                },
                (error) =>
                {
                    alert(error);
                }
            );
        }
    }, [runOnce, pageProps, projectId, problemId, problem.name]);

    /**
     * Whether the dialog has even been opened.
     */
    const [dialogOpened, setDialogOpened] = useState(false);
    return <>
        <div className="row">
            <div className="col">
                <h1>Problem</h1>
                <ProblemUpdateForm pageProps={pageProps} problem={problem} problemId={problemId} projectId={projectId} />
                
                <h2>Bets</h2>
                <CardGrid data={problem.bets} AddNewClick={(e) => { PageFunctions.DisplayModal(e, dialogOpened, (newValue) => { setDialogOpened(newValue) }) }} />
                <ProblemBetNewForm
                    title="Add Bet"
                    description="Use the form to quickly add new bets that you hope will help solve the problem. These can be fleshed out after being created."
                    buttonText="Add Bet"
                    saveAction={async (values, setSubmitting, resetForm, setErrors) =>
                    {
                        let saveResponse = await BetService.Put(projectId, problemId, JSON.stringify(values));

                        // We have finished submitting the form.
                        setSubmitting(false);

                        if (saveResponse.status !== 202)
                        {
                            const json: { message: string } = await saveResponse.json();
                            setErrors({ name: json.message });

                            ProblemService.Get(projectId, problemId).then(
                                (data) =>
                                {
                                    // Sets the model against the page.
                                    setProblem(data);
                                },
                                (error) =>
                                {
                                    alert(error);
                                }
                            );
                        }
                        else
                        {
                            pageProps.globalMessage({ message: "You have added a bet", class: "alert-success" });

                            ProblemService.Get(projectId, problemId).then(
                                (data) =>
                                {
                                    // Sets the model against the page.
                                    setProblem(data);

                                    // Resets the add new problem form.
                                    resetForm({});

                                    // Close the dialog.
                                    PageFunctions.CloseDialog('newModal');
                                },
                                (error) =>
                                {
                                    alert(error);
                                }
                            );
                        }
                    }} />
            </div>
        </div>
    </>;
}