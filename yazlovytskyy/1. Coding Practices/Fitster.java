//by WardCunningham

private void openFile() {
        stopCellEditing();

        if (chooser == null) {
            chooser = new JFileChooser();
            chooser.setDialogTitle("Choose a directory of test files");
            chooser.setFileSelectionMode(JFileChooser.FILES_AND_DIRECTORIES);
        }
        int returnVal = chooser.showOpenDialog(this);
        if(returnVal == JFileChooser.APPROVE_OPTION) {
            File selection = chooser.getSelectedFile();
            AbstractNode node;
            if (selection.isDirectory())
                node = new FolderNode(selection);
            else
                node = new FileNode(selection);
            node.fitster = this;
            fixturesTree.setModel(new DefaultTreeModel(node, true));
        }
    }
	
private void saveFile() {
        stopCellEditing();

        try {
            File selection = selectedFileNode().file;
            Parse tables = selectedFileNode().tables;
            PrintWriter pw = new PrintWriter(new FileWriter(selection));
            tables.print(new PrintWriter(pw));
            pw.close();
        } catch (IOException e) {
            openAlert("Can't write file", e);
        }
    }
