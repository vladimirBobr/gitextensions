using GitUI.CommandsDialogs;

namespace GitUITests.CommandsDialogs;

public sealed class FormCheckoutBranchTests
{
    [Test]
    public void Empty_name_should_be_translated_to_empty_string()
    {
        Translate(previousBranchName: "", branchNamesInNewMode: ["main"]).Should().Be("");
    }

    [Test]
    public void Whitespace_name_should_be_treated_as_empty()
    {
        Translate(previousBranchName: "   ", branchNamesInNewMode: ["main"]).Should().Be("");
    }

    [Test]
    public void Name_existing_in_the_new_list_should_be_kept_as_is()
    {
        Translate(previousBranchName: "main", branchNamesInNewMode: ["main", "dev"]).Should().Be("main");
    }

    [Test]
    public void Local_branch_existing_in_the_new_list_should_strip_the_remote_prefix()
    {
        Translate(previousBranchName: "origin/feature", branchNamesInNewMode: ["main", "feature"]).Should().Be("feature");
    }

    [Test]
    public void Nested_local_branch_existing_in_the_new_list_should_strip_the_remote_prefix()
    {
        Translate(previousBranchName: "origin/feature/login", branchNamesInNewMode: ["main", "feature/login"]).Should().Be("feature/login");
    }

    [Test]
    public void Remote_branch_existing_in_the_new_list_should_add_the_remote_prefix()
    {
        Translate(previousBranchName: "feature", branchNamesInNewMode: ["origin/feature", "upstream/dev"]).Should().Be("origin/feature");
    }

    [Test]
    public void Nested_remote_branch_existing_in_the_new_list_should_add_the_remote_prefix()
    {
        Translate(previousBranchName: "feature/login", branchNamesInNewMode: ["origin/feature/login"]).Should().Be("origin/feature/login");
    }

    [Test]
    public void Several_matching_remote_branches_should_use_the_first_match()
    {
        Translate(previousBranchName: "feature", branchNamesInNewMode: ["upstream/feature", "origin/feature"]).Should().Be("upstream/feature");
    }

    [Test]
    public void Unknown_name_should_be_kept_as_free_text()
    {
        Translate(previousBranchName: "demo/2026-11-11", branchNamesInNewMode: ["main", "dev"]).Should().Be("demo/2026-11-11");
    }

    [Test]
    public void Unknown_remote_name_should_be_kept_as_free_text()
    {
        Translate(previousBranchName: "origin/demo/2026-11-11", branchNamesInNewMode: ["main", "dev"]).Should().Be("origin/demo/2026-11-11");
    }

    [Test]
    public void Name_matching_only_a_partial_suffix_should_be_kept_as_free_text()
    {
        Translate(previousBranchName: "feature", branchNamesInNewMode: ["origin/feature2"]).Should().Be("feature");
    }

    [Test]
    public void Name_with_an_empty_new_list_should_be_kept_as_free_text()
    {
        Translate(previousBranchName: "feature", branchNamesInNewMode: []).Should().Be("feature");
    }

    [Test]
    public void Name_differing_only_in_case_should_be_kept_as_free_text()
    {
        Translate(previousBranchName: "Feature", branchNamesInNewMode: ["origin/feature"]).Should().Be("Feature");
    }

    [Test]
    public void Name_with_an_unrelated_shorter_local_branch_should_be_kept_as_free_text()
    {
        Translate(previousBranchName: "origin/feature/login", branchNamesInNewMode: ["login"]).Should().Be("origin/feature/login");
    }

    [Test]
    public void Name_with_an_unrelated_deeper_remote_branch_should_be_kept_as_free_text()
    {
        Translate(previousBranchName: "login", branchNamesInNewMode: ["origin/feature/login"]).Should().Be("login");
    }

    [Test]
    public void Stripping_the_remote_prefix_should_prefer_the_full_remainder()
    {
        Translate(previousBranchName: "origin/a/b", branchNamesInNewMode: ["b", "a/b"]).Should().Be("a/b");
    }

    [Test]
    public void Adding_the_remote_prefix_should_work_for_deeply_nested_names()
    {
        Translate(previousBranchName: "feature/login/x", branchNamesInNewMode: ["origin/feature/login/x"]).Should().Be("origin/feature/login/x");
    }

    private static string Translate(string previousBranchName, string[] branchNamesInNewMode)
        => FormCheckoutBranch.TranslateBranchName(previousBranchName, branchNamesInNewMode);
}
