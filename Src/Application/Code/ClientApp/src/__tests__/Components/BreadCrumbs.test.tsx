import { screen } from '@testing-library/react';
import ReactDOM from 'react-dom';
import { act } from 'react-dom/test-utils';
import { BreadCrumbItem, BreadCrumbs } from '../../Components/BreadCrumbs';
import
{
  BrowserRouter as Router,
} from "react-router-dom";

let container: HTMLDivElement | null;

beforeEach(() => {
  container = document.createElement('div');
  document.body.appendChild(container);
});

afterEach(() =>
{
  if (container !== null)
  {
    document.body.removeChild(container);
    container = null; 
  }
});

describe(`The breadcrumb component`, () => {
  it('can render with one item in it', () => {
    act(() => {
      let breadCrumbData = new Array<BreadCrumbItem>();
      breadCrumbData.push({
        address: "Address",
        text: "Name",
        isLast: true
      });
      let data = {
        breadCrumbs: breadCrumbData
      };
      ReactDOM.render(
        <Router>
          <BreadCrumbs {...data} />
        </Router>,
        container);
    });
    const linkElement = screen.getByText(/Name/i);
    expect(linkElement).toBeInTheDocument();
  });

  it('can render with two items in it', () => {
    act(() => {
      let breadCrumbData = new Array();
      breadCrumbData.push({
        address: "Address",
        text: "Name1"
      });
      breadCrumbData.push({
        address: "Address",
        text: "Name2"
      });
      let data = {
        breadCrumbs: breadCrumbData
      };
      ReactDOM.render(
        <Router>
          <BreadCrumbs {...data} />
        </Router>,
        container);
    });
    expect(screen.getByText(/Name1/i)).toBeInTheDocument();
    expect(screen.getByText(/Name2/i)).toBeInTheDocument();
  });

  it('can render with no items in it', () => {
    act(() => {
      let breadCrumbData = new Array();
      let data = {
        breadCrumbs: breadCrumbData
      };
      ReactDOM.render(
        <Router>
          <BreadCrumbs {...data} />
        </Router>,
        container);
    });
  });
});
