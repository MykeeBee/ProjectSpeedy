import { Dispatch } from "react";
import { BreadCrumbItem } from "../Components/BreadCrumbs";
import { CardItem } from "../Components/CardGrid";
import { IGlobalMessage } from "./Components";

/**
 * This is the interface for the parameters passed to every page.
 */
export interface IPage
{
    /**
     * Lets you update the pages breadcrumbs.
     */
    setBreadCrumbs: Dispatch<BreadCrumbItem[]>;

    /**
     * Lets you get the current page breadcrumbs.
     */
    breadCrumbs: BreadCrumbItem[];

    /**
     * Function you can call to display a message at the top of the page.
     */
    globalMessage: (alertMessage: IGlobalMessage) => void;
}

/**
 * Contains the model that powers the project page.
 */
export interface IProject
{
    /**
     * The name of the project.
     */
    name: string;

    /**
     * List of problems linked to the project.
     */
    problems: CardItem[];

    /**
     * Whether the data has been loaded into the page.
     */
    isLoaded: boolean;
}
